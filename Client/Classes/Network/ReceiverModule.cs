using Common.Network;
using Common.Network.Clients;
using Common.Network.Packets.MediaServerPackets;
using Network;

namespace Client.Classes.Network
{
    public class ReceiverModule
    {
        private readonly Routing _routing;
        public ReceiverModule(Routing routing) => _routing = routing;
        public void OnReceive(IClient client, Header header, byte[] data)
        {
            _routing.InvokeRoute((PacketIds)header.GetId(), client, data);
        }
    }
}
