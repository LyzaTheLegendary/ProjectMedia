using Common.Network.Clients;

namespace Client.Classes
{
    public static class Routes
    {
        public static void LoginResult(IClient client, byte[] data) => Gui.Gui.RequestGuiAction(data);
       
    }
}
