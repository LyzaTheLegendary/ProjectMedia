using System.Data;
using Terminal.Gui;

namespace Common.Gui.Server
{
    public static class Display
    {
        private static TerminalLogic? _instance;
        public static void ConstructInstance() 
        {
            _instance = new TerminalLogic();
            Task.Run(() => Application.Run());
        }
        private static TerminalLogic GetInstance() => _instance ?? throw new Exception("Initialize instance before calling functions!");
        public static void WriteNet(string msg) => GetInstance().WriteNet(msg);
    }
    public class TerminalLogic : Window
    {
        TextView _networkLog;
        TextView _localLog;
        TextView _errorLog;
        TableView _tableView;
        DataTable _table;
        public TerminalLogic() {
            Application.Init();
            
            MenuBar menu = new(new MenuBarItem[]
            {
                new MenuBarItem ("Logging", new MenuItem[]
                {
                    new MenuItem("Network", "Network information",() => { EnableLog(_networkLog); }),
                    new MenuItem("Local", "Local application information" ,() => { EnableLog(_localLog); }),
                    new MenuItem("Errors", "Error information" ,() => { EnableLog(_errorLog); }),
                })
            });
            Application.Top.Add(this);
            AddLogs();
            //Add(_networkLog);
            //Add(_localLog);
            //Add(_errorLog);
            Add(menu);

            _tableView = new()
            {
                Y = 1,
                X = 90,
                //Width = Dim.Fill() - 75,
                Width = 30,
                Height = Dim.Fill(),
            };
            
            _table = new DataTable();
            _table.Columns.Add("Connected Clients", typeof(string));
            //_table.Columns.Add("Port", typeof(string));
            //_table.Rows.Add("test", "test");
            _tableView.Table = _table;

            Add(_tableView);
            
        }
        public void AddConn(string address, ushort port) => _table.Rows.Add($"{address}:{port}");
        public void WriteNet(string msg) {
            _networkLog.Text += msg;
        }
        public void WriteLocal(string msg) { }
        public void WriteError(string msg) { }

        private void AddLogs()
        {

            ScrollView scrollView = new ScrollView()
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            
            _networkLog = new()
            {
                Y = 1,
                X = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Visible = false,
                ReadOnly = true,
                WordWrap = true,
                ColorScheme = new ColorScheme()
                {
                    Normal = new Terminal.Gui.Attribute(Color.Black, Color.White),
                    //HotNormal = new Terminal.Gui.Attribute(Color.Black, Color.BrightYellow)
                }
            };
            _localLog = new()
            {
                Y = 1,
                X = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Visible = false,
                ReadOnly = true,
                WordWrap = true,
                ColorScheme = new ColorScheme()
                {
                    Normal = new Terminal.Gui.Attribute(Color.Black, Color.White),
                    //HotNormal = new Terminal.Gui.Attribute(Color.Black, Color.BrightYellow)
                }
            };
            _errorLog = new()
            {
                Y = 1,
                X = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Visible = false,
                ReadOnly = true,
                WordWrap = true,
                ColorScheme = new ColorScheme()
                {
                    Normal = new Terminal.Gui.Attribute(Color.Black, Color.White),
                    //HotNormal = new Terminal.Gui.Attribute(Color.Black, Color.BrightYellow)
                }
            };
            scrollView.Add(_networkLog);
            scrollView.Add(_localLog);
            scrollView.Add(_errorLog);

            Add(scrollView); // TODO: figure out why it doesn't show up
        }
        private void EnableLog(TextView view)
        {
            bool currentState = view.Visible;
            _networkLog.Visible = false;
            _localLog.Visible = false;
            _errorLog.Visible = false;
            view.Visible = !currentState;
        }
    }
}
