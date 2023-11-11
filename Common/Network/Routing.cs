using Common.Network.Clients;
using Common.Network.Packets.MediaServerPackets;
using Network;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Common.Network
{
    public struct Routing
    {
        private readonly ConcurrentDictionary<ushort, Action<Header, IClient, byte[]>> _routes;
        public Routing() => _routes = new();
        public void InvokeRoute(ushort id, Header header, IClient client, byte[] data)
        {
            _routes.TryGetValue(id, out var action);
            if (action != null)
                action.Invoke(header, client, data);
#if DEBUG
            else
                Debug.Print($"Failed to find route for id: {id}");
#endif
        }
        public void AddRoute(ushort id, Action<Header, IClient, byte[]> action) => _routes.TryAdd(id, action);
        
    }
}
