using Common.Utilities;
using Network;
using System.Net.Sockets;
using System.Runtime.InteropServices;
namespace Common.Network.Clients
{
    public class Client : IClient
    {
        private readonly Socket _socket;
        private readonly Addr _addr;
        private readonly ID _id;
        private readonly CancellationTokenSource _tokenSource;
        private Action<Client>? _onDisconnect;
        public Client(string host, int port)
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(host, port);
            _addr = new Addr(host, (ushort)port);
            _tokenSource = new CancellationTokenSource();

            byte[] intBuff = new byte[sizeof(int)];

            _socket.Receive(intBuff);
            _id = new ID(MarshalUtil.BytesToStruct<int>(intBuff));
        }
        public Client(Socket socket, ID id)
        {
            _socket = socket;
            _id = id;
            _tokenSource = new CancellationTokenSource();
            string[] hostInfo = socket.RemoteEndPoint!.ToString()!.Split(':'); // wont be empty as it's used remotely
            _addr = new Addr(hostInfo[0], ushort.Parse(hostInfo[1]));

            Send(id);
        }
        public void Receive(Action<IClient, Header, byte[]> onReceive)
        {
            Task.Factory.StartNew(() =>
            {
                while (_socket.Connected)
                {
                    byte[] buff = new byte[Marshal.SizeOf<Header>()];
                    _socket.Receive(buff);

                    Header header = buff.Cast<Header>();
                    
                    buff = new byte[header.GetSize()];
                    _socket.Receive(buff);

                    Task.Run(() => onReceive(this, header, buff), _tokenSource.Token);
                }
            }, _tokenSource.Token);
        }
        public bool Connected()
            => _socket.Connected;
        public void Disconnect()
        {
            if (_onDisconnect != null)
                _onDisconnect(this);

            _tokenSource.Cancel();
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
        public Addr GetAddr()
            => _addr;
        public ID GetId()
            => _id;
        public void AddOnDisconnect(Action<Client> onDisconnect) 
            => _onDisconnect = onDisconnect;
        public void Send(byte[] buff)
        {
            if(_socket.Connected)
                _socket.Send(buff);
        }
        public void Send<T>(T structure)
        {
            if(_socket.Connected)
                _socket.Send(MarshalUtil.StructToBytes(structure));
        }
    }
}
