using Client.Gui;
using Client.Gui.Views;
using Common.Network.Clients;
using Gtk;
using System.Diagnostics;

public static class Program
{
    static readonly Process process = Process.GetCurrentProcess();
    static Thread logicThread;
    [STAThread]
    public static void Main(string[] args)
    {
        Addr host = new(args[0]);

        
        Application.Init();
        Gui.Construct("unnamed");
        
        logicThread = new Thread(() => { StartLogic(host); });
        logicThread.Start();
        Application.Run();
    }
    public static void StartLogic(Addr host)
    {
        //
        Thread.Sleep(100);
        Gui.SetView(new LoginView());
    }
}