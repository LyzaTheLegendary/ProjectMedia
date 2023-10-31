using Gtk;

namespace Client.Gui
{
    public interface IView
    {
        public void ReceiveMessage(byte[] data);
        public Widget GetContainer();
        public void Delete();
    }
}
