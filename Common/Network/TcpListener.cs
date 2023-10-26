using Common.Network.Clients;
using System.Net;
using System.Net.Sockets;

namespace Common.Network
{
    public class TcpListener
    {
        private Socket _socket;
        private Addr _addr;
        private bool isAlive = false;
        private IDPool _idPool;
        public TcpListener(Addr addr, int backLog)
        {
            _idPool = new IDPool();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(addr.GetIP()), addr.GetPort()); ;
            _socket = new(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _socket.Bind(ep);
            _socket.Listen(backLog);

            _addr = addr;
            isAlive = true;
        }
        public Addr GetAddr() 
            => _addr;
        public void Stop()
        {
            _socket.Close();
            isAlive = false;
        }
        public void Listen(Action<Client> onConnect)
        {
            Task.Factory.StartNew(() => {
                Thread.GetCurrentProcessorId();
                while (isAlive)
                    onConnect(new Client(_socket.Accept(), _idPool.GetNewID()));
                
            });
        }
    }
}
