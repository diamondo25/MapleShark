namespace MapleShark
{
    public class Opcode
    {
        public ushort Header { get; private set; }
        public bool Outbound { get; private set; }

        public Opcode(bool pOutbound, ushort pHeader)
        {
            Outbound = pOutbound;
            Header = pHeader;
        }
    }
}