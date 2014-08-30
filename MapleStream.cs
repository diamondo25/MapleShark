using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapleShark
{
    [Flags]
    public enum TransformLocale : int
    {
        AES = 1 << 1,
        MAPLE_CRYPTO = 1 << 2,
        OLD_KMS_CRYPTO = 1 << 7,
        KMS_CRYPTO = 1 << 8,
        NONE = 0
    }

    public sealed class MapleStream
    {
        private const int DEFAULT_SIZE = 4096;

        private bool mOutbound = false;
        public MapleAES mAES = null;
        private byte[] mBuffer = new byte[DEFAULT_SIZE];
        private int mCursor = 0;

        public MapleStream(bool pOutbound, ushort pBuild, byte pLocale, byte[] pIV, byte pSubVersion) { 
            mOutbound = pOutbound;
            mAES = new MapleAES(pBuild, pLocale, pIV, pSubVersion);
        }

        public void Append(byte[] pBuffer) { Append(pBuffer, 0, pBuffer.Length); }
        public void Append(byte[] pBuffer, int pStart, int pLength)
        {
            if (mBuffer.Length - mCursor < pLength)
            {
                int newSize = mBuffer.Length * 2;
                while (newSize < mCursor + pLength) newSize *= 2;
                Array.Resize<byte>(ref mBuffer, newSize);
            }
            Buffer.BlockCopy(pBuffer, pStart, mBuffer, mCursor, pLength);
            mCursor += pLength;
        }

        public MaplePacket Read(DateTime pTransmitted, ushort pBuild, byte pLocale)
        {
            if (mCursor < 4) return null;
            if (!mAES.ConfirmHeader(mBuffer, 0))
            {
                throw new Exception("Failed to confirm packet header");
            }

            ushort packetSize = mAES.GetHeaderLength(mBuffer, 0, pBuild == 255 && pLocale == 1);
            if (mCursor < (packetSize + 4)) 
                return null;
            byte[] packetBuffer = new byte[packetSize];
            Buffer.BlockCopy(mBuffer, 4, packetBuffer, 0, packetSize);

            bool byteheader = false;
            if ((pBuild == 40 && pLocale == 5) || (pBuild == 15 && pLocale == 7))
            { 
                // WvsBeta
                Decrypt(packetBuffer, pBuild, pLocale, TransformLocale.MAPLE_CRYPTO);
                byteheader = true;
            }
            else if (pLocale == 1 && pBuild == 255)
            { 
                // KMSB lol
                Decrypt(packetBuffer, pBuild, pLocale, TransformLocale.OLD_KMS_CRYPTO);
                byteheader = true;
                // Still reset header.
                mAES.ShiftIVOld();
            }
            else if (pLocale == 1 || pLocale == 2)
            { 
                // KMS / KMST
                Decrypt(packetBuffer, pBuild, pLocale, TransformLocale.KMS_CRYPTO);
            }
            else if (pLocale == 6 || pLocale == 4 || pLocale == 3 || (pLocale == 8 && pBuild >= 149))
            {
                // TWMS / CMS / JMS / GMS (>= 149)
                Decrypt(packetBuffer, pBuild, pLocale, TransformLocale.AES);

                mAES.ShiftIV();
            }
            else
            { 
                // All others lol
                Decrypt(packetBuffer, pBuild, pLocale, TransformLocale.AES | TransformLocale.MAPLE_CRYPTO);
            }

            mCursor -= (packetSize + 4);
            if (mCursor > 0) Buffer.BlockCopy(mBuffer, packetSize + 4, mBuffer, 0, mCursor);
            ushort opcode;

            if (byteheader)
            {
                opcode = (ushort)(packetBuffer[0]);
                Buffer.BlockCopy(packetBuffer, 1, packetBuffer, 0, packetSize - 1);
                Array.Resize(ref packetBuffer, packetSize - 1);
            }
            else
            {
                opcode = (ushort)(packetBuffer[0] | (packetBuffer[1] << 8));
                Buffer.BlockCopy(packetBuffer, 2, packetBuffer, 0, packetSize - 2);
                Array.Resize(ref packetBuffer, packetSize - 2);
            }

            Definition definition = Config.Instance.GetDefinition(pBuild, pLocale, mOutbound, opcode);
            return new MaplePacket(pTransmitted, mOutbound, pBuild, pLocale, opcode, definition == null ? "" : definition.Name, packetBuffer);
        }

        private void Decrypt(byte[] pBuffer, ushort pBuild, byte pLocale, TransformLocale pTransformLocale)
        {
            if ((pTransformLocale & TransformLocale.AES) != 0)
            {
                mAES.TransformAES(pBuffer);
            }

            if ((pTransformLocale & TransformLocale.MAPLE_CRYPTO) != 0)
            {
                for (int index1 = 1; index1 <= 6; ++index1)
                {
                    byte firstFeedback = 0;
                    byte secondFeedback = 0;
                    byte length = (byte)(pBuffer.Length & 0xFF);
                    if ((index1 % 2) == 0)
                    {
                        for (int index2 = 0; index2 < pBuffer.Length; ++index2)
                        {
                            byte temp = pBuffer[index2];
                            temp -= 0x48;
                            temp = (byte)(~temp);
                            temp = temp.RollLeft(length & 0xFF);
                            secondFeedback = temp;
                            temp ^= firstFeedback;
                            firstFeedback = secondFeedback;
                            temp -= length;
                            temp = temp.RollRight(3);
                            pBuffer[index2] = temp;
                            --length;
                        }
                    }
                    else
                    {
                        for (int index2 = pBuffer.Length - 1; index2 >= 0; --index2)
                        {
                            byte temp = pBuffer[index2];
                            temp = temp.RollLeft(3);
                            temp ^= 0x13;
                            secondFeedback = temp;
                            temp ^= firstFeedback;
                            firstFeedback = secondFeedback;
                            temp -= length;
                            temp = temp.RollRight(4);
                            pBuffer[index2] = temp;
                            --length;
                        }
                    }
                }

                mAES.ShiftIV();
            }
            
            if (pTransformLocale == TransformLocale.OLD_KMS_CRYPTO)
            {
                mAES.TransformOldKMS(pBuffer);
            }
            if (pTransformLocale == TransformLocale.KMS_CRYPTO)
            {
                mAES.TransformKMS(pBuffer);
            }
        }
    }
}