using Common.Database;
using Common.Network.Clients;
using Common.Utilities;
using Project_Friend_Media.Core;
using System.Diagnostics;

internal static class Program
{
    static readonly Process process = new Process();
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

        Server server = new(host);

        process.WaitForExit();
    }
}