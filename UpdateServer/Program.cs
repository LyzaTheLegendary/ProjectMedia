using Common.Gui.Server;
using System.Diagnostics;
using Terminal.Gui;
using UpdateServer.Core.Network;
using Common.Network.Clients;
public sealed class Program
{
    private static Process process;
    private static void Main()
    {
        
        process = Process.GetCurrentProcess();
        Display.ConstructInstance();
        Thread.Sleep(1000);
        UpServer server = new(new Addr("127.0.0.1:25565"));
        
        process.WaitForExit();
        Exit();
    }
    public static void Exit()
    {
        Application.RequestStop();
        process.Close();
    }
}