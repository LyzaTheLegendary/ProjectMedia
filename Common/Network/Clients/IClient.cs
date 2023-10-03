namespace Common.Network.Clients
{
    public interface IClient
    {
        public void Send(byte[] buff);
        public void Send<T>(T structure);
        public void Disconnect();
        public bool Connected();
        public ID GetId();
        public Addr GetAddr();
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
        public override string ToString()
            => $"{_address}:{_port}";
    }
}
