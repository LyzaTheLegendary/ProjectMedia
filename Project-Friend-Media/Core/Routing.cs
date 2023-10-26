using Common.Network.Clients;
using Common.Network.Packets.MediaServerPackets;
using Network;
using System.Collections.Concurrent;

namespace Project_Friend_Media.Core
{
    public struct Routing
    {
        readonly ConcurrentDictionary<PacketIds, Action<IClient, byte[]>> _routes;
        public Routing(ConcurrentDictionary<PacketIds, Action<IClient, byte[]>> routes)
            => _routes = routes;

        public void InvokeRoute(PacketIds id, IClient client, byte[] data)
            => _routes[id]?.Invoke(client, data);

    }
}
