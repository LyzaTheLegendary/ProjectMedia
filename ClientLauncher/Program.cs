using Common.Network.Clients;
using System.Diagnostics;

public sealed class Program
{
    private static Process process;
    private static void Main()
    {
        process = Process.GetCurrentProcess();
        Client client = new("127.0.0.1:25565");

        process.WaitForExit();

    }
    public static void Exit()
    {
        process.Close();
    }
}