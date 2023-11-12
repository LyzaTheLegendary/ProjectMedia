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
        routing.AddRoute((ushort)Common.Network.Packets.MediaServerPackets.PacketIds.FRIEND_INFO, Routes.FriendInfo);
        routing.AddRoute((ushort)Common.Network.Packets.MediaServerPackets.PacketIds.TOKEN, Routes.SaveToken);
        routing.AddRoute((ushort)Common.Network.Packets.MediaServerPackets.PacketIds.FRIEND_REQUEST, Routes.FriendRequest);
        routing.AddRoute((ushort)Common.Network.Packets.MediaServerPackets.PacketIds.FRIEND_REQUEST_RESULT, Routes.OnFriendRequestResult);

        Globals.NetworkInit(new Client.Network.NetworkModule(host));
        Globals.ReceiverInit(routing);

        Application.Init();
        Gui.Construct("Exodium");
        //TODO: Check if token exists and if we can login first?
        
        Gui.SetView(new LoginView());
        logicThread.Start();
        Application.Run();
    }
    public static void StartLogic()
    {
        
    }
}