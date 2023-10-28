using Gtk;

namespace Client.Gui
{
    public interface IView
    {   
        public Widget[] GetAllWidgets();
        public void Delete();
    }
}
