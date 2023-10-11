using Common.FileSystem;
using Common.Gui.Server;
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
        }
        public void OnConnect(Client client)
        {
            lock (clientList) { clientList.Add(client); }
            client.AddOnDisconnect(OnDisconnect);
            client.Receive(OnMessage);
            Display.AddConn(client.GetAddr());

        }
        public void OnDisconnect(Client client)
        {
            lock (clientList) { clientList.Remove(client); }
            Display.DelConn(client.GetAddr());
        }
        public void OnMessage(IClient client, Header header, byte[] buff) { // should turn with byte[] into Memory
            // imemory that block buffer copies on a cast
            switch((TS_CS)header.GetId())
            {
                case TS_CS.VERSION:
                    // Version check?
                    ulong remoteVer = buff.Cast<MSG_VERSION>().ver;
                    ulong localVer = storage.GetLatestUpdateVer();
                    if(remoteVer == localVer)
                    {
                        client.Send((ushort)TS_SC.UPDATE_INFO,new MSG_UPDATE_STATUS(true,0));
                        return;
                    }

                    List<mFile> files = new();

                    while(remoteVer > localVer)
                        files.AddRange(storage.GetUpdate(remoteVer++));

                    pool.EnqueueTask(() =>
                    {
                        foreach (mFile file in files)
                            client.Send((ushort)TS_SC.UPDATE_FILE, (byte[])file);

                        client.Send((ushort)TS_SC.UPDATE_FINISHED, new MSG_UPDATE_FINISHED(new Addr("127.0.0.1:25566")));
                    });

                    break;
            }
        }
    }
}
