using Common.FileSystem;
using Common.Network.Clients;
using Common.Network.Packets.UpdateServerPackets;
using Common.Utilities;
using Network;
using System.Diagnostics;

public sealed class Program
{
    private static Process process = Process.GetCurrentProcess();
    private static List<mFile> files = new();
    public static int FilesLeft = 0;
    private static void Main()
    {
        Client client = new("127.0.0.1:25565");
        client.Receive(OnMessage);
        Thread.Sleep(1000);
        client.PendMessage((ushort)TS_CS.VERSION, new MSG_VERSION(0));
        process.WaitForExit();

    }
    public static void OnMessage(IClient client, Header header, byte[] buff)
    {
        switch((TS_SC)header.GetId()) {
            case TS_SC.UPDATE_FILE:
                mFile file = (mFile)buff;
                files.Add(file);
                Console.WriteLine($"Downloaded: {file.GetFileName()} size: {file.GetDataSize()}");
                //files.Add(new FileReader(buff).ReadFile());
                break;
            case TS_SC.UPDATE_FINISHED:
                Addr serverAddr = (Addr)buff.Cast<MSG_UPDATE_FINISHED>(); // doesn't work well as it isn't in order, We should make a send queue
                client.Disconnect();
                Console.WriteLine("Done updating!");
                break;
            case TS_SC.UPDATE_INFO:
                MSG_UPDATE_STATUS status = buff.Cast<MSG_UPDATE_STATUS>();
                FilesLeft = status.files;
                Console.WriteLine($"Server at version:{status.remoteVer} files to download: {FilesLeft}");
                break;
        }
    }
    public static void Exit()
    {
        process.Close();
    }
}