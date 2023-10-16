using Common.Network.Clients;
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
        public static void AddConn(Addr addr) => GetInstance().AddConn(addr);
        public static void DelConn(Addr addr) => GetInstance().RemoveConn(addr);
        public static void WriteNet(string msg) => GetInstance().WriteNet(msg + "\n");
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
            AddLogs();
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
            Add(menu);

            _tableView = new()
            {
                Y = 1,
                X = 90,
                //Width = Dim.Fill() - 75,
                Width = 30,
                Height = Dim.Fill(),
                ColorScheme = new ColorScheme()
                {
                    Normal = new Terminal.Gui.Attribute(Color.White, Color.Black),
                    HotNormal = new Terminal.Gui.Attribute(Color.Black, Color.BrightYellow)
                }
            };
            
            _table = new DataTable();
            _table.Columns.Add("Connected Clients", typeof(string));
   
            _tableView.Table = _table;

            Add(_tableView);
            
        }
        public void AddConn(Addr addr)
        {
            lock (_table)
            {
                _table.Rows.Add(addr.ToString());
                //Application.Refresh();
            }
        }
        public void RemoveConn(Addr addr)
        {
            lock (_table)
            {

                if (_table.AsEnumerable().Count() == 0)
                    return;
                _table.AsEnumerable().Where(row => ((string)row.ItemArray[0]) == addr.ToString()).First().Delete(); // very unstable

            }

        }
        public void DelConn(Addr addr) => throw new NotImplementedException("TODO:IMPLEMENT ME");
        public void WriteNet(string msg) {
            lock (_networkLog)
            {
                _networkLog.Text += msg;
                //Application.Refresh();
            }
        }
        public void WriteLocal(string msg) { }
        public void WriteError(string msg) { }

        private void AddLogs()
        {

            //ScrollView scrollView = new ScrollView()
            //{
            //    X = 0,
            //    Y = 1,
            //    Width = Dim.Fill(),
            //    Height = Dim.Fill()
            //};
            
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
            Add(_networkLog);
            Add(_localLog);
            Add(_errorLog);
            //scrollView.Add(_networkLog);
            //scrollView.Add(_localLog);
            //scrollView.Add(_errorLog);

            //Add(scrollView); // TODO: figure out why it doesn't show up
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
