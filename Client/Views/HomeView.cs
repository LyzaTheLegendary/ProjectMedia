using Classes;
using Client.Gui;
using Common.Network.Packets.MediaServerPackets;
using Gtk;

namespace Client.Views
{
    public class HomeView : IView
    {
        private readonly Widget _container;
        private readonly Entry friendSearch;
        private readonly Button addFriendButton;
        private readonly Button homeButton;
        public HomeView()
        {
            Gui.Gui.Resize(600, 800);
            Grid container = new();
            
            friendSearch = new Entry();
            addFriendButton = new Button("+");
            homeButton = new Button("Home");
            addFriendButton.Clicked += (_, _) => SendFriendRequest(friendSearch.Buffer.Text);

            container.Attach(homeButton, 5, 5, 26, 26);
            container.AttachNextTo(friendSearch, homeButton, PositionType.Right, 25, 25);
            container.AttachNextTo(addFriendButton, friendSearch, PositionType.Right, 25, 25);
            
            ScrolledWindow scrolledWindow = new ScrolledWindow();
            
            container.AttachNextTo(scrolledWindow, homeButton, PositionType.Bottom, 3, 1);


            _container = container;
        }
        public Widget GetContainer()
            => _container;
        public void SendFriendRequest(string tUsername)
        {
            //addFriendButton.Sensitive = false;
            MSG_FRIEND_REQUEST msg = new(tUsername);
            Globals.NetworkModule.PendMessage((ushort)PacketIds.FRIEND_REQUEST, msg);

        }
        public void HandleGuiRequest(byte[] data)
        {
            
        }
    }
}
