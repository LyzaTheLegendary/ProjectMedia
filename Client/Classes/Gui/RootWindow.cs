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
                Application.Invoke(delegate
                {
                    rootWindow.DeleteEvent += delegate { Application.Quit(); };
                    rootWindow.Add(view.GetContainer());
                });
                
            }
        }
        public void Resize(int width, int height)
        {
            lock (rootWindow)
            {
                Application.Invoke(delegate
                {
                    rootWindow.SetSizeRequest(width, height);
                });
            }
        }
        public void AddWidget(Widget widget)
        {
            lock (rootWindow)
                Application.Invoke(delegate
                {
                    rootWindow.Add(widget);
                });
        }
        public void RemoveWidget(Widget widget)
        {
            lock (rootWindow)
                Application.Invoke(delegate
                {
                    rootWindow.Remove(widget);
                });
        }
        public void CleanWindow()
        {
            lock (rootWindow)
            {
                Application.Invoke(delegate
                {
                    foreach (Widget widget in rootWindow.Children)
                        rootWindow.Remove(widget);
                });
            }
        }
    }
}
