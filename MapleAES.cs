using System;
using System.Security.Cryptography;

namespace MapleShark
{
    public sealed class MapleAES
    {
        private static byte[] sSecretKey = new byte[] {
            0x13, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0xB4, 0x00, 0x00, 0x00,
            0x1B, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x33, 0x00, 0x00, 0x00, 0x52, 0x00, 0x00, 0x00 
        };

        private static byte[] sShiftKey = new byte[] {
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

        internal MapleAES(ushort pBuild, byte pLocale, byte[] pIV)
        {
            mBuild = pBuild;
            if ((short)pBuild < 0) { // Second one
                pBuild = (ushort)(0xFFFF - pBuild);
            }
            if (pLocale == 8 && pBuild >= 118) // GMS uses random keys since 118!
                mAES.Key = GMSKeys.GetKeyForVersion(pBuild);
            else
                mAES.Key = sSecretKey;
            mAES.Mode = CipherMode.ECB;
            mAES.Padding = PaddingMode.PKCS7;
            mTransformer = mAES.CreateEncryptor();
            mIV = pIV;
        }

		public void ShiftIVOld() {
			mIV = BitConverter.GetBytes(214013 * BitConverter.ToUInt32(mIV, 0) + 2531011);
		}

        public bool ConfirmHeader(byte[] pBuffer, int pStart)
        {
            bool b = (pBuffer[pStart] ^ mIV[2]) == (mBuild & 0xFF) &&
                   (pBuffer[pStart + 1] ^ mIV[3]) == ((mBuild >> 8) & 0xFF);

            if (!b)
            {
                Console.WriteLine("Version: {0:X4}", mBuild);
                string buf = "IV [";
                foreach (byte b1 in mIV)
                {
                    buf += string.Format("{0:X2} ", b1);
                }
                buf = buf.Trim();
                buf += "]";
                Console.WriteLine(buf);
                {
                    string ret = "GOT [";
                    for (int i = 0; i < 4; i++) ret += string.Format("{0:X2} ", pBuffer[pStart + i]);
                    Console.WriteLine("{0}]", ret);
                }
                {
                    byte[] hdr = new byte[4];
                    GenerateHeader(hdr);
                    string ret = "NEEDED [";
                    for (int i = 0; i < 4; i++) ret += string.Format("{0:X2} ", hdr[i]);
                    Console.WriteLine("{0}]", ret);
                }
            }
            return b;
        }
        public ushort GetHeaderLength(byte[] pBuffer, int pStart, bool pOldHeader)
        {
			if (pOldHeader) {
				return BitConverter.ToUInt16(pBuffer, pStart + 2);
			}

            int length = (int)pBuffer[pStart] |
                         (int)(pBuffer[pStart + 1] << 8) |
                         (int)(pBuffer[pStart + 2] << 16) |
                         (int)(pBuffer[pStart + 3] << 24);
            length = (length >> 16) ^ (length & 0xFFFF);
            return (ushort)length;
        }
        public void GenerateHeader(byte[] pBuffer)
        {
            ushort build = (ushort)(((mBuild >> 8) & 0xFF) | ((mBuild << 8) & 0xFF00));
            build = (ushort)(((mIV[3] & 0xFF) | ((mIV[2] << 8) & 0xFF00)) ^ build);
            ushort length = (ushort)((((pBuffer.Length - 4) >> 8) & 0xFF) | (((pBuffer.Length - 4) << 8) & 0xFF00));
            length ^= build;
            pBuffer[0] = (byte)(build >> 8);
            pBuffer[1] = (byte)build;
            pBuffer[2] = (byte)(length >> 8);
            pBuffer[3] = (byte)length;
        }

		public void TransformKMS(byte[] pBuffer) {
			byte[] oudeIV = new byte[4];
			Buffer.BlockCopy(mIV, 0, oudeIV, 0, 4);
			for (int i = 0; i < pBuffer.Length; i++) {
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
            {
                pBuffer[i] = (byte)(16 * (mIV[0] ^ pBuffer[i]) | ((byte)(mIV[0] ^ pBuffer[i]) >> 4));
            }
            ShiftIVOld();
        }

		public void TransformAES(byte[] pBuffer) {
			int remaining = pBuffer.Length;
			int length = 0x5B0;
			int start = 0;
			byte[] realIV = new byte[mIV.Length * 4];
			while (remaining > 0) {
				for (int index = 0; index < realIV.Length; ++index) realIV[index] = mIV[index % 4];

				if (remaining < length) length = remaining;
				for (int index = start; index < (start + length); ++index) {
					if (((index - start) % realIV.Length) == 0) {
						byte[] tempIV = new byte[realIV.Length];
						mTransformer.TransformBlock(realIV, 0, realIV.Length, tempIV, 0);
						Buffer.BlockCopy(tempIV, 0, realIV, 0, realIV.Length);
						//realIV = mTransformer.TransformFinalBlock(realIV, 0, realIV.Length);
					}
					pBuffer[index] ^= realIV[(index - start) % realIV.Length];
				}
				start += length;
				remaining -= length;
				length = 0x5B4;
			}
			ShiftIV(mIV);
		}

        public void TransformJMS(byte[] pBuffer)
        {

        }

		private void Morph(byte inputByte, byte[] start) {
			byte a = start[1];
			byte b = a;
			uint c, d;
			b = sShiftKey[b];
			b -= inputByte;
			start[0] += b;
			b = start[2];
			b ^= sShiftKey[inputByte];
			a -= b;
			start[1] = a;
			a = start[3];
			b = a;
			a -= start[0];
			b = sShiftKey[b];
			b += inputByte;
			b ^= start[2];
			start[2] = b;
			a += sShiftKey[inputByte];
			start[3] = a;

			c = (uint)(start[0] + start[1] * 0x100 + start[2] * 0x10000 + start[3] * 0x1000000);
			d = c;
			c >>= 0x1D;
			d <<= 0x03;
			c |= d;
			start[0] = (byte)(c % 0x100);
			c /= 0x100;
			start[1] = (byte)(c % 0x100);
			c /= 0x100;
			start[2] = (byte)(c % 0x100);
			start[3] = (byte)(c / 0x100);
		}

        public void ShiftIV()
        {
            ShiftIV(mIV);
        }

        private void ShiftIV(byte[] pOldIV)
        {
            byte[] newIV = new byte[] { 0xF2, 0x53, 0x50, 0xC6 };
            for (int index = 0; index < mIV.Length; ++index)
            {
                byte temp1 = newIV[1];
                byte temp2 = sShiftKey[temp1];
				byte temp3 = pOldIV[index];
                temp2 -= temp3;
                newIV[0] += temp2;
                temp2 = newIV[2];
                temp2 ^= sShiftKey[temp3];
                temp1 -= temp2;
                newIV[1] = temp1;
                temp1 = newIV[3];
                temp2 = temp1;
                temp1 -= newIV[0];
                temp2 = sShiftKey[temp2];
                temp2 += temp3;
                temp2 ^= newIV[2];
                newIV[2] = temp2;
                temp1 += sShiftKey[temp3];
                newIV[3] = temp1;
                uint result1 = (uint)newIV[0] | ((uint)newIV[1] << 8) | ((uint)newIV[2] << 16) | ((uint)newIV[3] << 24);
                uint result2 = result1 >> 0x1D;
                result1 <<= 3;
                result2 |= result1;
                newIV[0] = (byte)(result2 & 0xFF);
                newIV[1] = (byte)((result2 >> 8) & 0xFF);
                newIV[2] = (byte)((result2 >> 16) & 0xFF);
                newIV[3] = (byte)((result2 >> 24) & 0xFF);
            }
            Buffer.BlockCopy(newIV, 0, mIV, 0, mIV.Length);
        }
        /*
        private void ShiftString(byte[] pOldIV)
        {
            byte[] newIV = new byte[] { 0xF2, 0x53, 0x50, 0xC6 };
            for (int index = 0; index < mIV.Length; ++index)
            {
                byte temp1 = newIV[1];
                byte temp2 = sShiftKey[temp1];
                byte temp3 = pOldIV[index];

                newIV[0] += (byte)(sShiftKey[newIV[1]] - temp2);
                newIV[1] = (byte)(temp3 - (newIV[2] * sShiftKey[temp3]));
                byte temp4 = (byte)(newIV[3] - newIV[0]);
                newIV[2] ^= (byte)(temp2 + sShiftKey[newIV[3]]);
                newIV[3] = (byte)(sShiftKey[temp2] + temp4);

                newIV[0] = newIV[0].RollLeft(3);
            }
            Buffer.BlockCopy(newIV, 0, mIV, 0, mIV.Length);
        }
         * */
    }
}
