using System.Diagnostics;
using UpdateServer.Core.Network;
using Common.Network.Clients;
public sealed class Program
{
    private static Process process = Process.GetCurrentProcess();
    private static void Main()
    {
        Thread.Sleep(1000);
        UpServer server = new(new Addr("127.0.0.1:25565"));
        
        process.WaitForExit();
        Exit();
    }
    public static void Exit()
    {
        process.Close();
    }
}