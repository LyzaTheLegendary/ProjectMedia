using Common.Network.Packets.UpdateServerPackets;
using Network;

namespace Common.Network.Clients
{
    public interface IClient
    {
        void PendMessage(byte[] buff);
        void PendMessage<T>(ushort id, T structure);
        void PendMessage(ushort id, byte[] buff);
        void Disconnect();
        bool Connected();
        Stream GetStream();
        ID GetId();
        Addr GetAddr();
    }
    public struct Addr
    {
        private readonly string _address;
        private readonly ushort _port;
        public Addr(string address, ushort port)
        {
            _address = address;
            _port = port;
        }
        public Addr(string address)
        {
            string[] addrInfo = address.Split(":");
            _address = addrInfo[0];
            _port = ushort.Parse(addrInfo[1]);
        }
        public string GetIP() => _address;
        public ushort GetPort() => _port;
        public override string ToString()
            => $"{_address}:{_port}";
    }
}
