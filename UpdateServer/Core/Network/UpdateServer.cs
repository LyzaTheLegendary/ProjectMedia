using Common.Network;
using Common.Network.Clients;
using Common.Network.Packets.UpdateServerPackets;
using Common.Threading;
using Common.Utilities;
using Network;

namespace UpdateServer.Core.Network
{
    public class UpdateServer
    {
        readonly TcpListener listener;
        readonly TaskPool pool;
        readonly List<Client> clientList = new(100);
        public UpdateServer(string host, int port)
        {
            pool = new(10);
            listener = new TcpListener(host,port,10);
            listener.Listen(OnConnect);

        }
        public void OnConnect(Client client)
        {
            lock (clientList) { clientList.Add(client); }
            client.AddOnDisconnect(OnDisconnect);

        }
        public void OnDisconnect(Client client)
        {
            lock (clientList) { clientList.Remove(client); }
        }
        public void OnMessage(IClient client, Header header, byte[] buff) { 
            switch((PacketIds)header.GetId())
            {
                case PacketIds.VERSION:
                    // Version check?-
                    ulong version = buff.Cast<MSG_VERSION>().ver;
                    break;
            }
        }
    }
}
