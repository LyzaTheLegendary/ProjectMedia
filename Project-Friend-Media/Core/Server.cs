using Common.Network;
using Common.Network.Clients;
using Common.Network.Packets.MediaServerPackets;
using Common.Threading;
using Network;
using Project_Friend_Media.Core.Users;

namespace Project_Friend_Media.Core
{
    public sealed class Server
    {
        private readonly List<IClient> _clients = new(100);
        private readonly TaskPool _taskPool = new(10);
        private readonly TcpListener _listener;
        private readonly Routing _routing;
        public Server(Addr host, Routing routing)
        {
            _routing = routing;
            _listener = new TcpListener(host, 10);
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
            UserManager.RemoveUser(client.GetId());
            lock (_clients)
                _clients.Remove(client);
        }
        public void OnMessage(IClient client, Header header, byte[] data) 
            => _taskPool.EnqueueTask(() => { _routing.InvokeRoute((PacketIds)header.GetId(), client, data); });
        
    }
}
