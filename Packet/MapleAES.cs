using System;
using System.Security.Cryptography;

namespace MapleShark
{
    public sealed class MapleAES
    {
        private readonly static byte[] sSecretKey = new byte[] {
            0x13, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0xB4, 0x00, 0x00, 0x00,
            0x1B, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x33, 0x00, 0x00, 0x00, 0x52, 0x00, 0x00, 0x00 
        };

        private readonly static byte[] sShiftKey = new byte[] {
            0xEC, 0x3F, 0x77, 0xA4, 0x45, 0xD0, 0x71, 0xBF, 0xB7, 0x98, 0x20, 0xFC, 0x4B, 0xE9, 0xB3, 0xE1,
            0x5C, 0x22, 0xF7, 0x0C, 0x44, 0x1B, 0x81, 0xBD, 0x63, 0x8D, 0xD4, 0xC3, 0xF2, 0x10, 0x19, 0xE0,
            0xFB, 0xA1, 0x6E, 0x66, 0xEA, 0xAE, 0xD6, 0xCE, 0x06, 0x18, 0x4E, 0xEB, 0x78, 0x95, 0xDB, 0xBA,
            0xB6, 0x42, 0x7A, 0x2A, 0x83, 0x0B, 0x54, 0x67, 0x6D, 0xE8, 0x65, 0xE7, 0x2F, 0x07, 0xF3, 0xAA,
            0x27, 0x7B, 0x85, 0xB0, 0x26, 0xFD, 0x8B, 0xA9, 0xFA, 0xBE, 0xA8, 0xD7, 0xCB, 0xCC, 0x92, 0xDA,
            0xF9, 0x93, 0x60, 0x2D, 0xDD, 0xD2, 0xA2, 0x9B, 0x39, 0x5F, 0x82, 0x21, 0x4C, 0x69, 0xF8, 0x31,
            0x87, 0xEE, 0x8E, 0xAD, 0x8C, 0x6A, 0xBC, 0xB5, 0x6B, 0x59, 0x13, 0xF1, 0x04, 0x00, 0xF6, 0x5A,
            0x35, 0x79, 0x48, 0x8F, 0x15, 0xCD, 0x97, 0x57, 0x12, 0x3E, 0x37, 0xFF, 0x9D, 0x4F, 0x51, 0xF5,
            0xA3, 0x70, 0xBB, 0x14, 0x75, 0xC2, 0xB8, 0x72, 0xC0, 0xED, 0x7D, 0x68, 0xC9, 0x2E, 0x0D, 0x62,
            0x46, 0x17, 0x11, 0x4D, 0x6C, 0xC4, 0x7E, 0x53, 0xC1, 0x25, 0xC7, 0x9A, 0x1C, 0x88, 0x58, 0x2C,
            0x89, 0xDC, 0x02, 0x64, 0x40, 0x01, 0x5D, 0x38, 0xA5, 0xE2, 0xAF, 0x55, 0xD5, 0xEF, 0x1A, 0x7C,
            0xA7, 0x5B, 0xA6, 0x6F, 0x86, 0x9F, 0x73, 0xE6, 0x0A, 0xDE, 0x2B, 0x99, 0x4A, 0x47, 0x9C, 0xDF,
            0x09, 0x76, 0x9E, 0x30, 0x0E, 0xE4, 0xB2, 0x94, 0xA0, 0x3B, 0x34, 0x1D, 0x28, 0x0F, 0x36, 0xE3,
            0x23, 0xB4, 0x03, 0xD8, 0x90, 0xC8, 0x3C, 0xFE, 0x5E, 0x32, 0x24, 0x50, 0x1F, 0x3A, 0x43, 0x8A,
            0x96, 0x41, 0x74, 0xAC, 0x52, 0x33, 0xF0, 0xD9, 0x29, 0x80, 0xB1, 0x16, 0xD3, 0xAB, 0x91, 0xB9,
            0x84, 0x7F, 0x61, 0x1E, 0xCF, 0xC5, 0xD1, 0x56, 0x3D, 0xCA, 0xF4, 0x05, 0xC6, 0xE5, 0x08, 0x49 
        };

        private ushort mBuild = 0;
        private RijndaelManaged mAES = new RijndaelManaged();
        private ICryptoTransform mTransformer = null;
        public byte[] mIV { get; private set; }

        internal MapleAES(ushort pBuild, byte pLocale, byte[] pIV, byte pSubVersion)
        {
            mBuild = pBuild;

            if ((short)pBuild < 0)
                pBuild = (ushort)(0xFFFF - pBuild);

            mAES.Key = MapleKeys.GetKeyForVersion(pLocale, pBuild, pSubVersion) ?? sSecretKey;

            mAES.Mode = CipherMode.ECB;
            mAES.Padding = PaddingMode.PKCS7;
            mTransformer = mAES.CreateEncryptor();
            mIV = pIV;
        }

        public void ShiftIVOld()
        {
            mIV = BitConverter.GetBytes(214013 * BitConverter.ToUInt32(mIV, 0) + 2531011);
        }

        public bool ConfirmHeader(byte[] pBuffer, int pStart)
        {
            bool b = (pBuffer[pStart] ^ mIV[2]) == (mBuild & 0xFF) &&
                   (pBuffer[pStart + 1] ^ mIV[3]) == ((mBuild >> 8) & 0xFF);
            return b;
        }

        public static int GetHeaderLength(byte[] pBuffer, int pBytesAvailable, bool pOldHeader)
        {
            if (pOldHeader) return 4;

            ushort ivBytes = (ushort)(pBuffer[0] | pBuffer[1] << 8);
            ushort xorredSize = (ushort)(pBuffer[2] | pBuffer[3] << 8);
            ushort length = (ushort)(xorredSize ^ ivBytes);

            if (length == 0xFF00) return 8;
            else return 4;
        }

        public static int GetPacketLength(byte[] pBuffer, int pBytesAvailable, bool pOldHeader)
        {
            if (pBytesAvailable < 4) return pBytesAvailable - 4;

            if (pOldHeader) return BitConverter.ToUInt16(pBuffer, 2);

            ushort ivBytes = (ushort)(pBuffer[0] | pBuffer[1] << 8);
            ushort xorredSize = (ushort)(pBuffer[2] | pBuffer[3] << 8);

            ushort length = (ushort)(xorredSize ^ ivBytes);

            if (length == 0xFF00)
            {
                if (pBytesAvailable < 8) return pBytesAvailable - 8;
                return BitConverter.ToInt32(pBuffer, 4) ^ ivBytes;
            }
            return length;
        }

        public void TransformKMS(byte[] pBuffer)
        {
            byte[] oudeIV = new byte[4];
            Buffer.BlockCopy(mIV, 0, oudeIV, 0, 4);
            for (int i = 0; i < pBuffer.Length; i++)
            {
                byte v7 = (byte)(pBuffer[i] ^ sShiftKey[mIV[0]]);
                byte v8 = (byte)((v7 >> 1) & 0x55 | 2 * (v7 & 0xD5));
                pBuffer[i] = (byte)(0x10 * v8 | (v8 >> 4));
                Morph(pBuffer[i], mIV);
            }

            ShiftIV(oudeIV);
        }

        // Done by csproj
        public void TransformOldKMS(byte[] pBuffer)
        {
            for (int i = 0; i < pBuffer.Length; i++)
                pBuffer[i] = (byte)(16 * (mIV[0] ^ pBuffer[i]) | ((byte)(mIV[0] ^ pBuffer[i]) >> 4));

            ShiftIVOld();
        }

        public void TransformAES(byte[] pData)
        {
            byte[] freshIVBlock = new byte[16] {
                mIV[0], mIV[1], mIV[2], mIV[3],
                mIV[0], mIV[1], mIV[2], mIV[3],
                mIV[0], mIV[1], mIV[2], mIV[3],
                mIV[0], mIV[1], mIV[2], mIV[3]
            };
            byte[] currentIVBlock = new byte[16];
            int dataSize = pData.Length;
            int startBlockSize = 1456;
            if (dataSize >= 0xFF00) startBlockSize -= 4;

            int blockSize = 0;
            for (int start = 0; start < dataSize; start += blockSize)
            {
                blockSize = Math.Min(start == 0 ? startBlockSize : 1460, dataSize - start);
                Buffer.BlockCopy(freshIVBlock, 0, currentIVBlock, 0, 16);

                for (int i = 0; i < blockSize; i++)
                {
                    // For every 16 bytes, update IV. 
                    if ((i % 16) == 0)
                    {
                        mTransformer.TransformBlock(currentIVBlock, 0, 16, currentIVBlock, 0);
                    }

                    pData[start + i] ^= currentIVBlock[i % 16];
                }
            }
        }
        public void ShiftIV(byte[] pOldIV = null)
        {
            if (pOldIV == null) pOldIV = mIV;

            byte[] newIV = new byte[] { 0xF2, 0x53, 0x50, 0xC6 };
            for (int i = 0; i < 4; ++i)
                Morph(pOldIV[i], newIV);

            Buffer.BlockCopy(newIV, 0, mIV, 0, mIV.Length);
        }

        public static void Morph(byte pValue, byte[] pIV)
        {
            byte input = pValue;
            byte tableInput = sShiftKey[input];
            pIV[0] += (byte)(sShiftKey[pIV[1]] - input);
            pIV[1] -= (byte)(pIV[2] ^ tableInput);
            pIV[2] ^= (byte)(sShiftKey[pIV[3]] + input);
            pIV[3] -= (byte)(pIV[0] - tableInput);

            uint val = (uint)(pIV[0] | pIV[1] << 8 | pIV[2] << 16 | pIV[3] << 24);
            val = (val >> 0x1D | val << 0x03);
            pIV[0] = (byte)(val & 0xFF);
            pIV[1] = (byte)((val >> 8) & 0xFF);
            pIV[2] = (byte)((val >> 16) & 0xFF);
            pIV[3] = (byte)((val >> 24) & 0xFF);
        }
    }
}