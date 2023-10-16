using Common.Threading;
using Common.Utilities;
using Network;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Runtime.InteropServices;
namespace Common.Network.Clients
{
    public class Client : IClient
    {
        private readonly Socket _socket;
        private readonly Addr _addr;
        private readonly ID _id;
        //private readonly CancellationTokenSource _tokenSource; // we move this to the TaskPool instead!
        private readonly TaskPool _pool = new(4);
        private readonly BlockingCollection<byte[]> _dataPool = new();
        private Action<Client>? _onDisconnect;
        public Client(string address)
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _addr = new Addr(address);
            _socket.Connect(_addr.GetIP(), _addr.GetPort());
            _pool.EnqueueTask(StartQueue);
            //_tokenSource = new CancellationTokenSource();

            byte[] intBuff = new byte[sizeof(int)];

            _socket.Receive(intBuff);
            _id = new ID(MarshalUtil.BytesToStruct<int>(intBuff));
            
        }
        public Client(Socket socket, ID id)
        {
            _socket = socket;
            _id = id;
            //_tokenSource = new CancellationTokenSource();
            _addr = new Addr(socket.RemoteEndPoint!.ToString()!);
            _pool.EnqueueTask(StartQueue);
            if (_socket.Connected)
                _socket.Send(MarshalUtil.StructToBytes(id.GetNumber()));
        }
        private void StartQueue()
        {
            foreach (byte[] buff in _dataPool.GetConsumingEnumerable()) // rapes memory?
            {
                try
                {
                    if (_socket.Connected)
                        _socket.Send(buff);
                }catch(Exception)
                {
                    Disconnect();
                }
            }
        }
        public void Receive(Action<IClient, Header, byte[]> onReceive)
        {
            _pool.EnqueueTask(() =>
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

                    Header header = buff.Cast<Header>(); // sometimes packet doesn't arrive completely?
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

                    _pool.EnqueueTask(() => onReceive(this, header, buff));
                }
            });
        }
        public bool Connected()
            => _socket.Connected;
        public void Disconnect()
        {
            if (_onDisconnect != null)
                _onDisconnect(this);

            _pool.Stop();
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
            catch(Exception) { }
        }
        public Stream GetStream() => new NetworkStream(_socket);
        public Addr GetAddr()
            => _addr;
        public ID GetId()
            => _id;
        public void AddOnDisconnect(Action<Client> onDisconnect) 
            => _onDisconnect = onDisconnect;
        public void PendMessage(byte[] buff) 
            => _dataPool.Add(buff);
        
        // rework the id system!     
        public void PendMessage<T>(ushort id,T structure)
        {
            byte[] buff = MarshalUtil.StructToBytes(structure);
            List<byte> packet = new(MarshalUtil.StructToBytes(new Header(id, buff.Length)));
            packet.AddRange(buff);

            _dataPool.Add(packet.ToArray());
        }

        public void PendMessage(ushort id, byte[] buff)
        {
            byte[] header = MarshalUtil.StructToBytes(new Header(id, buff.Length));
            List<byte> packet = new(buff.Length + Marshal.SizeOf<Header>());

            packet.AddRange(header);
            packet.AddRange(buff);

            _dataPool.Add(packet.ToArray());
        }
    }
}
