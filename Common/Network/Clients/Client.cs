using Common.Network.Packets;
using Common.Network.Packets.UpdateServerPackets;
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
        public Client(string address)
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _addr = new Addr(address);
            _socket.Connect(_addr.GetIP(), _addr.GetPort());
            
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
            _addr = new Addr(socket.RemoteEndPoint!.ToString()!);

            if (_socket.Connected)
                _socket.Send(BitConverter.GetBytes(id.GetNumber()));
        }
        public void Receive(Action<IClient, Header, byte[]> onReceive)
        {
            Task.Factory.StartNew(() =>
            {
                while (_socket.Connected)
                {
                    byte[] buff = new byte[Marshal.SizeOf<Header>()];
                    try
                    {
                        int receivedBytes = _socket.Receive(buff);
                        if (receivedBytes == 0)
                        {
                            _onDisconnect?.Invoke(this);
                            return;
                        }
                    }
                    catch (Exception ex) { _onDisconnect?.Invoke(this); return; }

                    Header header = buff.Cast<Header>();
                    
                    buff = new byte[header.GetSize()];
                    try
                    {
                        int receivedBytes2 = _socket.Receive(buff);
                        if (receivedBytes2 == 0)
                        {
                            _onDisconnect?.Invoke(this);
                            return;
                        }
                    }
                    catch (Exception ex) { _onDisconnect?.Invoke(this); return; }

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
        public Stream GetStream() => new NetworkStream(_socket);
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
        public void Send<T>(TS_SC id,T structure)
        {
            byte[] buff = MarshalUtil.StructToBytes(structure);
            List<byte> packet = new(MarshalUtil.StructToBytes(new Header((ushort)id,(uint)buff.Length)));
            packet.AddRange(buff);

            if (_socket.Connected)
                _socket.Send(buff.ToArray());
        }
        public void Send(TS_SC id, byte[] buff)
        {
            List<byte> packet = new(MarshalUtil.StructToBytes(new Header((ushort)id, (uint)buff.Length)));
            packet.AddRange(buff);

            if (_socket.Connected)
                _socket.Send(buff.ToArray());
        }
    }
}
