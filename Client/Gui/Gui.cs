using Gtk;

namespace Client.Gui
{
    public class Gui
    {
        private static RootWindow? instance;
        public static void Construct(string windowTitle) => instance = new RootWindow(windowTitle);
        public static void SetView(IView view) => instance.SetView(view);
        public static void Resize(int height, int width) => instance.Resize(width, height);
        public static void RemoveWidget(Widget widget) => instance.RemoveWidget(widget);
    }
}
