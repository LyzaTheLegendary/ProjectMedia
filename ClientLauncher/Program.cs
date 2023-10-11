using Common.Network.Clients;
using Common.Network.Packets.UpdateServerPackets;
using Network;
using System.Diagnostics;

public sealed class Program
{
    private static Process process;
    private static void Main()
    {
        process = Process.GetCurrentProcess();
        Client client = new("127.0.0.1:25565");
        client.Receive(OnMessage);
        Thread.Sleep(1000);
        client.Send((ushort)Common.Network.Packets.UpdateServerPackets.TS_CS.VERSION, new MSG_VERSION(1));
        process.WaitForExit();

    }
    public static void OnMessage(IClient client, Header header, byte[] buff)
    {

    }
    public static void Exit()
    {
        process.Close();
    }
}