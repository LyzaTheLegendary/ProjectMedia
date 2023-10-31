using Client.Network;

namespace Classes
{
    public static class Globals
    {
        public static NetworkModule NetworkModule { get; private set; }
        public static void Init(NetworkModule netModule) => NetworkModule = netModule;
    }
}
