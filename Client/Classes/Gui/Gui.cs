using Gtk;

namespace Client.Gui
{
    public class Gui
    {
        private static RootWindow? instance;
        private static IView? currentView;
        public static void Construct(string windowTitle) => instance = new RootWindow(windowTitle);
        public static void SetView(IView view)
        {
            currentView = view;
            instance!.SetView(view);
        }
        public static void RequestGuiAction(byte[] data) => currentView?.HandleGuiRequest(data);
        public static void Resize(int height, int width) => instance?.Resize(width, height);
        public static void RemoveWidget(Widget widget) => instance?.RemoveWidget(widget);
    }
}
