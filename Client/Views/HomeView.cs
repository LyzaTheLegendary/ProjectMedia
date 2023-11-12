using Classes;
using Client.Gui;
using Client.Views.Components.HomeView;
using Common.Network.Packets;
using Common.Network.Packets.MediaServerPackets;
using Common.Utilities;
using Gtk;

namespace Client.Views
{
    public class HomeView : IView
    {
        private readonly Widget _container;
        private readonly Entry friendSearch;
        private readonly Button addFriendButton;
        private readonly Button homeButton;

        private readonly ChannelList channels;
        public HomeView()
        {
            Gui.Gui.Resize(600, 800);
            Grid container = new();
            
            friendSearch = new Entry();
            friendSearch.PlaceholderText = "Add friend";
            addFriendButton = new Button("+");
            homeButton = new Button("Home");
            addFriendButton.Clicked += (_, _) => SendFriendRequest(friendSearch.Buffer.Text);

            container.Attach(homeButton, 5, 5, 26, 26);
            container.AttachNextTo(friendSearch, homeButton, PositionType.Right, 25, 25);
            container.AttachNextTo(addFriendButton, friendSearch, PositionType.Right, 25, 25);

            channels = new ChannelList();
            container.AttachNextTo(channels.GetContainer(), homeButton, PositionType.Bottom, 100, 500);


            _container = container;
        }
        public Widget GetContainer()
            => _container;
        public void SendFriendRequest(string tUsername)
        {
            if (tUsername.Length > 50)
                return; // TODO create error message field for friend requests

            Globals.NetworkModule.PendMessage((ushort)PacketIds.FRIEND_REQUEST, tUsername.ToAsciiBytes(), (code) =>
            {
                switch (code)
                {
                    case ResultCodes.Success:
                        Console.WriteLine("Friend request sent!");
                        break;
                    case ResultCodes.NotFound:
                        Console.WriteLine("User does not exist!");
                        break;
                    case ResultCodes.AlreadyExist:
                        Console.WriteLine("Already sent a friend request to that user!");
                        break;
                    case ResultCodes.Denied:
                        Console.WriteLine("Can't send a friend request to yourself!");
                        break;
                }
            });

        }
        public void HandleGuiRequest(byte[] data)
        {
            
        }
    }
}
