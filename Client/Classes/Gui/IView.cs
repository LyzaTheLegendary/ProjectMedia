using Common.Network.Packets.MediaServerPackets;
using Gtk;

namespace Client.Gui
{
    public interface IView
    {
        public Widget GetContainer();
        public void HandleGuiRequest(byte[] data);
    }
}
