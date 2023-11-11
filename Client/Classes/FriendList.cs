using Classes;
using Common.Network.Packets.MediaServerPackets;

namespace Client.Classes
{
    public struct Friend
    {
        private readonly string _username;
        public Friend(string username) => _username = username;
        public string GetUsername() => _username;
    }
    public class FriendList
    {
        private static List<Friend> _friends = new(10);
        private static List<Friend> _requests = new(10);
        public static void AddFriend(Friend friend) => _friends.Add(friend);
        public static void AcceptFriend(string username)
        {
            Friend friend = _requests.Where(friend => friend.GetUsername() == username).First();
            Globals.NetworkModule.PendMessage((ushort)PacketIds.FRIEND_REQUEST_ACCEPT, new MSG_FRIEND_REQUEST_ACCEPTED(friend.GetUsername()));

            _requests.Remove(friend);
            _friends.Add(friend);
        }
        public static Friend[] GetAllFriends => _friends.ToArray();
    }
}
