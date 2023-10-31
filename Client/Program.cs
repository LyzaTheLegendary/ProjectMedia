using Classes;
using Client.Gui;
using Client.Gui.Views;
using Common.Network.Clients;
using Gtk;
using System.Diagnostics;

public static class Program
{
    static readonly Process process = Process.GetCurrentProcess();
    static Thread logicThread = new(StartLogic);
    [STAThread]
    public static void Main(string[] args)
    {
        Addr host = new(args[0]);

        Globals.Init(new Client.Network.NetworkModule(host));
        Application.Init();
        Gui.Construct("unnamed");
        
        logicThread.Start();
        Application.Run();
    }
    public static void StartLogic()
    {
        Gui.SetView(new LoginView());
    }
}