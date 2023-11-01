using Common.Network.Clients;
using Common.Network.Packets.MediaServerPackets;
using System.Collections.Concurrent;

namespace Common.Network
{
    public struct Routing
    {
        readonly ConcurrentDictionary<PacketIds, Action<IClient, byte[]>> _routes;
        public Routing() => _routes = new();
        public void InvokeRoute(PacketIds id, IClient client, byte[] data) => _routes[id]?.Invoke(client, data);
        public void AddRoute(PacketIds id, Action<IClient, byte[]> action) => _routes.TryAdd(id, action);
        
    }
}
