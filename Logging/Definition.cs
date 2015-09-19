namespace MapleShark
{
    public sealed class Definition
    {
        public ushort Build = 0;
        public byte Locale = 0;
        public bool Outbound = false;
        public ushort Opcode = 0;
        public string Name = "";
        public bool Ignore = false;

        public override string ToString()
        {
            return "Locale: " + Locale + "; Build: " + Build + "; Name: " + Name + "; Opcode: 0x" + Opcode.ToString("X4") + "; Outbound: " + Outbound + "; Ignored: " + Ignore;
        }
    }
}
