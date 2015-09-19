using System;
using System.Text;

namespace MapleShark
{
    public sealed class StructureSegment
    {
        private byte[] mBuffer;
        private Encoding encoding = Encoding.UTF8;

        public StructureSegment(byte[] pBuffer, int pStart, int pLength, byte locale)
        {
            mBuffer = new byte[pLength];
            try
            {
                switch (locale)
                {
                    case MapleLocale.KOREA:
                    case MapleLocale.KOREA_TEST:
                        encoding = Encoding.GetEncoding(51949); // EUC_KR
                        break;
                    case MapleLocale.JAPAN:
                        encoding = Encoding.GetEncoding(50222); // Shift_JIS
                        break;
                    case MapleLocale.CHINA:
                        encoding = Encoding.GetEncoding(54936); // GB18030
                        break;
                    case MapleLocale.TESPIA:
                        encoding = Encoding.Default;
                        break;
                    case MapleLocale.TAIWAN:
                        encoding = Encoding.GetEncoding("BIG5-HKSCS");
                        break;
                    default:
                        encoding = Encoding.UTF8;
                        break;
                }
            }
            catch
            {
                encoding = Encoding.UTF8;
            }
            Buffer.BlockCopy(pBuffer, pStart, mBuffer, 0, pLength);
        }

        public byte? Byte { get { if (mBuffer.Length >= 1) return mBuffer[0]; return null; } }
        public sbyte? SByte { get { if (mBuffer.Length >= 1) return (sbyte)mBuffer[0]; return null; } }
        public ushort? UShort { get { if (mBuffer.Length >= 2) return BitConverter.ToUInt16(mBuffer, 0); return null; } }
        public short? Short { get { if (mBuffer.Length >= 2) return BitConverter.ToInt16(mBuffer, 0); return null; } }
        public uint? UInt { get { if (mBuffer.Length >= 4) return BitConverter.ToUInt32(mBuffer, 0); return null; } }
        public int? Int { get { if (mBuffer.Length >= 4) return BitConverter.ToInt32(mBuffer, 0); return null; } }
        public ulong? ULong { get { if (mBuffer.Length >= 8) return BitConverter.ToUInt64(mBuffer, 0); return null; } }
        public long? Long { get { if (mBuffer.Length >= 8) return BitConverter.ToInt64(mBuffer, 0); return null; } }
        public long? FlippedLong
        {
            get
            {
                if (mBuffer.Length >= 8)
                {
                    long time = BitConverter.ToInt64(mBuffer, 0);
                    time = (long)(
                        ((time << 32) & 0xFFFFFFFF) |
                        (time & 0xFFFFFFFF)
                        );
                    return time;
                }
                return null;
            }
        }
        public string IpAddress
        {
            get
            {

                if (mBuffer.Length == 4)
                {
                    return string.Format("{0}.{1}.{2}.{3}", mBuffer[0].ToString(), mBuffer[1].ToString(), mBuffer[2].ToString(), mBuffer[3].ToString());
                }

                return null;
            }
        }

        public DateTime? Date
        {
            get
            {
                try
                {
                    if (mBuffer.Length >= 8)
                        return DateTime.FromFileTimeUtc(BitConverter.ToInt64(mBuffer, 0));
                }
                catch { }
                return null;
            }
        }

        public DateTime? FlippedDate
        {
            get
            {
                try
                {
                    if (mBuffer.Length >= 8)
                    {
                        long time = BitConverter.ToInt64(mBuffer, 0);
                        time = (long)(
                            ((time << 32) & 0xFFFFFFFF) |
                            (time & 0xFFFFFFFF)
                            );

                        return DateTime.FromFileTimeUtc(time);
                    }
                }
                catch { }
                return null;
            }
        }

        public string String
        {
            get
            {
                if (mBuffer.Length == 0) return null;
                if (mBuffer[0] == 0x00) return "";
                for (int index = 0; index < mBuffer.Length; ++index) if (mBuffer[index] == 0x00) return encoding.GetString(mBuffer, 0, index);
                return encoding.GetString(mBuffer, 0, mBuffer.Length);
            }
        }

        public string StringIgnore
        {
            get
            {
                byte[] sBuffer = new byte[mBuffer.Length];
                Buffer.BlockCopy(mBuffer, 0, sBuffer, 0, mBuffer.Length);
                if (sBuffer.Length == 0) return null;
                for (int index = 0; index < sBuffer.Length; ++index) if (sBuffer[index] >= 0x00 && sBuffer[index] < 0x20) sBuffer[index] = 0x2E;
                return encoding.GetString(sBuffer, 0, sBuffer.Length);
            }
        }

        public string Length
        {
            get
            {
                return mBuffer.Length + (mBuffer.Length != 1 ? " bytes" : " byte");
            }
        }
    }
}
