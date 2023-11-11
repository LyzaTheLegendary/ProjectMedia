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
            UserManager.SetClientGetter(GetClientViaId);
            Console.WriteLine($"Listening on {_listener.GetAddr()}");
        }
        private void OnConnect(Client client)
        {
            client.AddOnDisconnect(OnDisconnect);
            client.Receive(OnMessage);
            lock (_clients)
                _clients.Add(client);
            Console.WriteLine($"Client connected {client.GetAddr()}");
        }
        public void OnDisconnect(Client client)
        {
            UserManager.RemoveUser(client.GetId());
            lock (_clients)
                _clients.Remove(client);
            Console.WriteLine($"Client disconnected {client.GetAddr()}");
        }
        public void OnMessage(IClient client, Header header, byte[] data) 
            => _taskPool.EnqueueTask(() => { _routing.InvokeRoute(header.GetId(), header, client, data); });
        private IClient GetClientViaId(ID id)
            =>  _clients.Where(client => client.GetId() == id).First();        
    }
}
