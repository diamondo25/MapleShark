using System.Windows.Forms;

namespace MapleShark
{
    public sealed class DoubleBufferedListView : ListView
    {
        public DoubleBufferedListView()
            : base()
        {
            DoubleBuffered = true;
        }
    }
}