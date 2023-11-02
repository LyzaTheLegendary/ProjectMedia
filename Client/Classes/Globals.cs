using Client.Classes.Network;
using Client.Network;
using Common.Network;

namespace Classes
{
    public static class Globals
    {
        public static NetworkModule NetworkModule { get; private set; }
        public static ReceiverModule ReceiverModule { get; private set; }
        public static void NetworkInit(NetworkModule netModule) => NetworkModule = netModule;
        public static void ReceiverInit(Routing routes)
        {
            ReceiverModule = new ReceiverModule(routes);
            NetworkModule.OnReceive(ReceiverModule.OnReceive);

        }

    }
}
