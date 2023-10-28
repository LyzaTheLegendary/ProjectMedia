using Common.FileSystem;
using Common.Network;
using Common.Network.Clients;
using Common.Network.Packets.UpdateServerPackets;
using Common.Threading;
using Common.Utilities;
using Network;
using UpdateServer.Core.FileSystem;

namespace UpdateServer.Core.Network
{
    public class UpServer
    {
        readonly TcpListener listener;
        readonly TaskPool pool;
        readonly List<Client> clientList = new(100);
        readonly Storage storage;
        public UpServer(Addr addr)
        {
            storage = new Storage("FileSystem");
           
            pool = new(10);
            listener = new TcpListener(addr,10);
            listener.Listen(OnConnect);
            Console.WriteLine($"Listening on address: {addr}");
        }
        public void OnConnect(Client client)
        {
            lock (clientList) { clientList.Add(client); }
            client.AddOnDisconnect(OnDisconnect);
            client.Receive(OnMessage);

        }
        public void OnDisconnect(Client client)
        {
            lock (clientList) { clientList.Remove(client); }
        }
        public void OnMessage(IClient client, Header header, byte[] buff) { // should turn with byte[] into Memory
            // imemory that block buffer copies on a cast
            switch((TS_CS)header.GetId())
            {
                case TS_CS.VERSION:
                    // Version check?
                    int remoteVer = buff.Cast<MSG_VERSION>().ver;
                    int localVer = storage.GetLatestUpdateVer();
                    if (remoteVer >= localVer)
                    {
                        client.PendMessage((ushort)TS_SC.UPDATE_FINISHED, new MSG_UPDATE_FINISHED(new Addr("127.0.0.1:25566")));
                        return;
                    }

                    List<mFile> files = new(100);
                    

                    while(remoteVer < (localVer - 1)) { 
                        mFile[]? tempFiles = storage.GetUpdate(remoteVer);
                        if(tempFiles != null)
                            files.AddRange(tempFiles);
                        remoteVer++;
                    }

                    client.PendMessage((ushort)TS_SC.UPDATE_INFO, new MSG_UPDATE_STATUS(false, files.Count, localVer - 1));

                    pool.EnqueueTask(() =>
                    {
                        foreach (mFile file in files) {
                            //Thread.Sleep(20);
                            Console.WriteLine($"Sending file: {file.GetFileName()} to {client.GetAddr()}");
                            client.PendMessage((ushort)TS_SC.UPDATE_FILE, (byte[])file);
                            }
                        files.Clear();

                        Addr host = new("127.0.0.1:25566");
                        client.PendMessage((ushort)TS_SC.UPDATE_FINISHED, new MSG_UPDATE_FINISHED(host));
                        Console.WriteLine($"Update finished, sending to server: {host}");
                        //GC.Collect();
                        
                        client.Disconnect();
                    });

                    break;
            }
        }
    }
}
