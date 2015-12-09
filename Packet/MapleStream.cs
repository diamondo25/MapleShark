using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapleShark
{
    [Flags]
    public enum TransformMethod : int
    {
        AES = 1 << 1,
        MAPLE_CRYPTO = 1 << 2,
        OLD_KMS_CRYPTO = 1 << 3,
        KMS_CRYPTO = 1 << 4,

        SHIFT_IV = 1 << 5,
        SHIFT_IV_OLD = 1 << 6,
        NONE = 0
    }

    public sealed class MapleStream
    {
        private const int DEFAULT_SIZE = 4096;

        private bool mOutbound = false;
        private MapleAES mAES = null;
        private byte[] mBuffer = new byte[DEFAULT_SIZE];
        private int mCursor = 0;
        private int _expectedDataSize = 4;

        private TransformMethod _transformMethod;
        private bool _usesByteHeader = false;
        private bool _usesOldHeader = false;

        public ushort Build { get; private set; }
        public byte Locale { get; private set; }

        public MapleStream(bool pOutbound, ushort pBuild, byte pLocale, byte[] pIV, byte pSubVersion)
        {
            mOutbound = pOutbound;
            Build = pBuild;
            Locale = pLocale;

            if (mOutbound)
                mAES = new MapleAES(Build, Locale, pIV, pSubVersion);
            else
                mAES = new MapleAES((ushort)(0xFFFF - Build), Locale, pIV, pSubVersion);

            if ((Locale == MapleLocale.TESPIA && Build == 40) ||
                (Locale == MapleLocale.SOUTH_EAST_ASIA && Build == 15))
            {
                // WvsBeta
                _transformMethod = TransformMethod.MAPLE_CRYPTO | TransformMethod.SHIFT_IV;
                _usesByteHeader = true;
            }
            else if (Locale == MapleLocale.KOREA_TEST && Build == 255)
            {
                // KMSB (Modified client)
                _transformMethod = TransformMethod.OLD_KMS_CRYPTO | TransformMethod.SHIFT_IV_OLD;
                _usesByteHeader = true;
                _usesOldHeader = true;
            }
            else if (
                Locale == MapleLocale.TAIWAN ||
                Locale == MapleLocale.CHINA ||
                Locale == MapleLocale.TESPIA ||
                Locale == MapleLocale.JAPAN ||
                (Locale == MapleLocale.GLOBAL && (short)Build >= 149) ||
                (Locale == MapleLocale.KOREA && Build >= 221) ||
                (Locale == MapleLocale.SOUTH_EAST_ASIA && Build >= 144) ||
                (Locale == MapleLocale.EUROPE && Build >= 115))
            {
                // TWMS / CMS / CMST / JMS / GMS (>= 149)
                _transformMethod = TransformMethod.AES | TransformMethod.SHIFT_IV;
            }
            else if (Locale == MapleLocale.KOREA || Locale == MapleLocale.KOREA_TEST)
            {
                // KMS / KMST
                _transformMethod = TransformMethod.KMS_CRYPTO;
            }
            else
            {
                // All others lol
                _transformMethod = TransformMethod.AES | TransformMethod.MAPLE_CRYPTO | TransformMethod.SHIFT_IV;
            }

            Console.WriteLine("Using transform methods: {0}", _transformMethod);
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

        public MaplePacket Read(DateTime pTransmitted)
        {
            if (mCursor < _expectedDataSize) return null;
            if (!mAES.ConfirmHeader(mBuffer, 0))
            {
                throw new Exception("Failed to confirm packet header");
            }

            int headerLength = MapleAES.GetHeaderLength(mBuffer, mCursor, _usesOldHeader);
            _expectedDataSize = headerLength;
            if (mCursor < headerLength)
            {
                return null;
            }

            int packetSize = MapleAES.GetPacketLength(mBuffer, mCursor, _usesOldHeader);
            _expectedDataSize = packetSize + headerLength;
            if (mCursor < (packetSize + headerLength))
            {
                return null;
            }

            byte[] packetBuffer = new byte[packetSize];
            Buffer.BlockCopy(mBuffer, headerLength, packetBuffer, 0, packetSize);

            var preDecodeIV = BitConverter.ToUInt32(mAES.mIV, 0);

            Decrypt(packetBuffer, _transformMethod);

            var postDecodeIV = BitConverter.ToUInt32(mAES.mIV, 0);

            mCursor -= _expectedDataSize;
            if (mCursor > 0) Buffer.BlockCopy(mBuffer, _expectedDataSize, mBuffer, 0, mCursor);
            ushort opcode;

            if (_usesByteHeader)
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

            _expectedDataSize = 4;

            Definition definition = Config.Instance.GetDefinition(Build, Locale, mOutbound, opcode);
            return new MaplePacket(pTransmitted, mOutbound, Build, Locale, opcode, definition == null ? "" : definition.Name, packetBuffer, preDecodeIV, postDecodeIV);
        }

        private void Decrypt(byte[] pBuffer, TransformMethod pTransformLocale)
        {
            if ((pTransformLocale & TransformMethod.AES) != 0) mAES.TransformAES(pBuffer);

            if ((pTransformLocale & TransformMethod.MAPLE_CRYPTO) != 0)
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
                            temp = RollLeft(temp, length & 0xFF);
                            secondFeedback = temp;
                            temp ^= firstFeedback;
                            firstFeedback = secondFeedback;
                            temp -= length;
                            temp = RollRight(temp, 3);
                            pBuffer[index2] = temp;
                            --length;
                        }
                    }
                    else
                    {
                        for (int index2 = pBuffer.Length - 1; index2 >= 0; --index2)
                        {
                            byte temp = pBuffer[index2];
                            temp = RollLeft(temp, 3);
                            temp ^= 0x13;
                            secondFeedback = temp;
                            temp ^= firstFeedback;
                            firstFeedback = secondFeedback;
                            temp -= length;
                            temp = RollRight(temp, 4);
                            pBuffer[index2] = temp;
                            --length;
                        }
                    }
                }
            }

            if ((pTransformLocale & TransformMethod.KMS_CRYPTO) != 0) mAES.TransformKMS(pBuffer);
            if ((pTransformLocale & TransformMethod.OLD_KMS_CRYPTO) != 0) mAES.TransformOldKMS(pBuffer);

            if ((pTransformLocale & TransformMethod.SHIFT_IV) != 0) mAES.ShiftIV();
            if ((pTransformLocale & TransformMethod.SHIFT_IV_OLD) != 0) mAES.ShiftIVOld();
        }

        public static byte RollLeft(byte pThis, int pCount)
        {
            uint overflow = ((uint)pThis) << (pCount % 8);
            return (byte)((overflow & 0xFF) | (overflow >> 8));
        }

        public static byte RollRight(byte pThis, int pCount)
        {
            uint overflow = (((uint)pThis) << 8) >> (pCount % 8);
            return (byte)((overflow & 0xFF) | (overflow >> 8));
        }
    }
}
