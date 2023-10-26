using Common.Network;
using Common.Network.Clients;
using Common.Network.Packets.MediaServerPackets;
using Common.Threading;
using Network;

namespace Project_Friend_Media.Core
{
    public sealed class Server
    {
        private readonly List<IClient> _clients = new(100);
        private readonly TcpListener _listener;
        private readonly TaskPool _taskPool;
        private readonly Routing _routing;
        public Server(Addr host)
        {
            _listener = new TcpListener(host, 10);
            _taskPool = new TaskPool(10);
            _listener.Listen(OnConnect);
        }
        public void OnConnect(Client client)
        {
            client.AddOnDisconnect(OnDisconnect);
            client.Receive(OnMessage);
            lock (_clients)
                _clients.Add(client);
        }
        public void OnDisconnect(Client client)
        {
            lock (_clients)
                _clients.Remove(client);
        }
        public void OnMessage(IClient client, Header header, byte[] data)
        {
            //_routes[header.GetId()]?.invoke(client, header, data);
            // vs 
            switch ((PacketIds)header.GetId())
            {
                case PacketIds.LOGIN:
                    
                    break;
            }
        }
    }
}
