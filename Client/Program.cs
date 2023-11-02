using Classes;
using Client.Classes;
using Client.Gui;
using Client.Gui.Views;
using Common.Network;
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

        Routing routing = new();
        routing.AddRoute(Common.Network.Packets.MediaServerPackets.PacketIds.LOGIN_RESULT, Routes.LoginResult);

        Globals.NetworkInit(new Client.Network.NetworkModule(host));
        Globals.ReceiverInit(routing);

        Application.Init();
        Gui.Construct("unnamed");
        Gui.SetView(new LoginView());
        logicThread.Start();
        Application.Run();
    }
    public static void StartLogic()
    {
        
    }
}