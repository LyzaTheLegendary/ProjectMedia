using Common.Network.Clients;
using Common.Network.Packets;
using Common.Network.Packets.MediaServerPackets;
using Common.Threading;
using Common.Utilities;
using Network;
using TcpClient = Common.Network.Clients.Client;
namespace Client.Network
{
    public enum ConnState
    {
        CONNECTED,
        DISCONNECTED,
        RECONNECTING,
    }
    public class NetworkModule // introduce reconnecting
    {
        private readonly IClient _client;
        private readonly ConnState _state;
        private readonly System.Timers.Timer _heartBeatTimer;
        public NetworkModule(Addr host) {
            _client = new TcpClient(host);
            _state = ConnState.CONNECTED;
            _heartBeatTimer = new System.Timers.Timer();
            _heartBeatTimer.Interval = 10000;
            _heartBeatTimer.AutoReset = true;
            _heartBeatTimer.Elapsed += HeartBeatTimer_Elapsed;
            _heartBeatTimer.Start();
        }

        private void HeartBeatTimer_Elapsed(object? obj, System.Timers.ElapsedEventArgs e) 
            => PendMessage((ushort)PacketIds.HEARTBEAT, new MSG_HEARTBEAT(new mTime(DateTime.Now)));
        public void Reconnect(Addr host)
        {
            throw new NotImplementedException();
        }
        public void OnReceive(Action<IClient, Header, byte[]> onReceive) => ((TcpClient)_client).Receive(onReceive);
        public void PendMessage<T>(ushort id, T packet) where T : struct => _client.PendMessage(id, packet);
        public void PendMessage<T>(ushort id, T packet, Action<ResultCodes> action) where T : struct => _client.PendMessage(id, packet,action);
        public ConnState GetState() => _state;
    }
}
