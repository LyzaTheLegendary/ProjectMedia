using Gtk;

namespace Client.Gui
{
    public class RootWindow
    {
        private readonly Window rootWindow;
        public RootWindow(string rootWindowTitle)
        {
            rootWindow = new Window(rootWindowTitle);
            rootWindow.ShowAll();
        }
        ~RootWindow() 
            => rootWindow.Dispose();
        public void SetView(IView view)
        {
            CleanWindow();
            lock (rootWindow)
            {
                foreach (Widget widget in view.GetAllWidgets())
                    rootWindow.Add(widget);
            }
        }
        public void Resize(int width, int height)
        {
            lock (rootWindow)
            {
                rootWindow.SetSizeRequest(width, height);
            }
        }
        public void AddWidget(Widget widget)
        {
            lock (rootWindow)
                rootWindow.Add(widget);
        }
        public void RemoveWidget(Widget widget)
        {
            lock (rootWindow)
                rootWindow.Remove(widget);
        }
        public void CleanWindow()
        {
            lock (rootWindow)
            {
                foreach (Widget widget in rootWindow.Children)
                    rootWindow.Remove(widget);
            }
        }
    }
}
