﻿using Common.Database;
using Common.Network;
using Common.Network.Clients;
using Common.Network.Packets.MediaServerPackets;
using Common.Utilities;
using Project_Friend_Media;
using Project_Friend_Media.Core;
using System.Diagnostics;

internal static class Program
{
    static readonly Process process = Process.GetCurrentProcess();
    static void Main()
    {
        Settings.Init("settings.txt");
        DbConn.Init(
            Settings.GetValue("db.host"), 
            Settings.GetValue("db.user"), 
            Settings.GetValue("db.pass"), 
            Settings.GetValue("db.name"));

        DbWorker.Init(int.Parse(Settings.GetValue("db.poolSize")));
        Addr host = new(Settings.GetValue("server.address"));

        Routing routing = new();
        routing.AddRoute((ushort)PacketIds.HEARTBEAT, Routes.OnHeartBeat);
        routing.AddRoute((ushort)PacketIds.LOGIN, Routes.LoginUser);
        routing.AddRoute((ushort)PacketIds.FRIEND_REQUEST, Routes.AddFriend);

        Server server = new(host,routing);

        process.WaitForExit();
    }
}