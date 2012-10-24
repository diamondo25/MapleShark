using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapleShark
{
    public enum TransformLocale
    {
        SPECIAL,
        AES,
        AES_MCRYPTO,
        MCRYPTO,
        NONE,
    }

    public sealed class MapleStream
    {
        private const int DEFAULT_SIZE = 4096;

        private bool mOutbound = false;
        private MapleAES mAES = null;
        private byte[] mBuffer = new byte[DEFAULT_SIZE];
        private int mCursor = 0;

        public MapleStream(bool pOutbound, ushort pBuild, byte[] pIV) { mOutbound = pOutbound; mAES = new MapleAES(pBuild, pIV); }

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
            if (mCursor < (packetSize + 4)) return null;
            byte[] packetBuffer = new byte[packetSize];
            Buffer.BlockCopy(mBuffer, 4, packetBuffer, 0, packetSize);
            bool byteheader = false;
            if (pBuild == 40 && pLocale == 5)
            { // WvsBeta
                Decrypt(packetBuffer, pBuild, pLocale, TransformLocale.MCRYPTO);
                byteheader = true;
            }
            else if (pLocale == 1 && pBuild == 255)
            { // KMSB lol
                Decrypt(packetBuffer, pBuild, pLocale, TransformLocale.NONE);
                byteheader = true;
                // Still reset header.
                mAES.ShiftIVOld();
            }
            else if (pLocale == 1 || pLocale == 2)
            { // KMS / KMST
                Decrypt(packetBuffer, pBuild, pLocale, TransformLocale.SPECIAL);
            }
            else if (pLocale == 8 && pBuild >= 118)
            {
                if (pBuild >= 120)
                    mAES.ChangeKey(new byte[] { 0x8D, 0x00, 0x00, 0x00, 0xD5, 0x00, 0x00, 0x00, 0xD0, 0x00, 0x00, 0x00, 0x2B, 0x00, 0x00, 0x00, 0xA8, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00, 0x00, 0x74, 0x00, 0x00, 0x00, 0xFE, 0x00, 0x00, 0x00});
                else if (pBuild >= 118)
                    mAES.ChangeKey(new byte[] {0x5A, 0x00, 0x00, 0x00, 0x22, 0x00, 0x00, 0x00, 0xFB, 0x00, 0x00, 0x00, 0xD1, 0x00, 0x00, 0x00,
            0x8F, 0x00, 0x00, 0x00, 0x93, 0x00, 0x00, 0x00, 0xCD, 0x00, 0x00, 0x00, 0xE6, 0x00, 0x00, 0x00});
                Decrypt(packetBuffer, pBuild, pLocale, TransformLocale.AES_MCRYPTO);
            }
            else
            { // All others lol
                Decrypt(packetBuffer, pBuild, pLocale, TransformLocale.AES_MCRYPTO);
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
            if (pTransformLocale == TransformLocale.AES)
            {
                mAES.TransformAES(pBuffer);
            }
            else if (pTransformLocale == TransformLocale.AES_MCRYPTO)
            {
                mAES.TransformAES(pBuffer);
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
            }
            else if (pTransformLocale == TransformLocale.NONE)
            {
                // lol
            }
            else if (pTransformLocale == TransformLocale.MCRYPTO)
            {
                mAES.ShiftIV();
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
            }
            else if (pTransformLocale == TransformLocale.SPECIAL)
            {
                mAES.TransformKMS(pBuffer);
            }
        }
    }
}