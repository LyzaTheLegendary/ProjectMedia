using Common.FileSystem.Storage;

using Gtk;

namespace Client.Gui
{
    public sealed class MainWIndow : Window
    {
        private bool _dragging = false;
        private int _dragX, _dragY;
        public MainWIndow(string title) : base(title)
        {
            SetDefaultSize(800, 600);
            Maximize();
            DeleteEvent += delegate { ; };
            // Create a new CSS provider
            var provider = new CssProvider();

            // Load the CSS file
            provider.LoadFromData(Storage.GetFile("MainStyle.css").GetText());

            // Apply the styles to the window
            StyleContext.AddProviderForScreen(Gdk.Screen.Default, provider, 800);
            //StyleContext.AddClass("background-red");

            //var childWindow = new Window(WindowType.Toplevel)
            //{
            //    Title = "Child Window",
            //    DefaultWidth = 200,
            //    DefaultHeight = 200

            var container = new Layout(null,null);
            container.StyleContext.AddClass("background-red");
            // Add some widgets to the container
            var button = new Button("Drag me!");
            container.Put(button, 0, 0);
            // Attach event handlers to the container
            //container.GrabNotify += Container_GrabNotify;
            container.MotionNotifyEvent += OnMotionNotify;

            // Add the container to the main window
            Add(container);

            // Show all windows
            ShowAll();
            //CssProvider provider = new();
            //provider.LoadFromData(Storage.GetFile("MainStyle.css").GetText());
            // Add the child window to the main window
            //Add(childWindow);
            //StyleContext.AddProviderForScreen(Gdk.Screen.Default, provider, 800);
            //StyleContext.AddProvider
            //StyleContext.AddClass("red-background");
            //Grid grid = new();
            //grid.Add(childWindow);
            //Alignment alignment = new (0.5f, 0.5f, 0f, 0f);
            //alignment.Add(grid);
            //Add(alignment);

            //Label usernameLabel = new Label("Username");
            //Entry usernameInput = new();
            //grid.Attach(usernameLabel, 0, 0, 1, 1);
            //grid.Attach(usernameInput, 0, 2, 1, 1);            

            //Label passwordLabel = new Label("Password");
            //Entry passwordInput = new();
            //grid.Attach(passwordLabel, 0, 8, 1, 1);
            //grid.Attach(passwordInput, 0, 10, 1, 1);


            ShowAll();
        }

        private void OnMotionNotify(object sender, MotionNotifyEventArgs args)
        {
            var container = (Layout)sender;
            Button btn = (Button)container.Children[0];
            Console.WriteLine($"X:{(int)args.Event.XRoot} Y:{(int)args.Event.YRoot}");
            container.Move(btn, (int)args.Event.XRoot, (int)args.Event.YRoot);
        }
    }
}
