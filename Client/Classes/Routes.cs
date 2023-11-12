using Common.FileSystem.Storage;
using Common.Network.Clients;
using Common.Network.Packets;
using Common.Network.Packets.MediaServerPackets;
using Common.Utilities;
using Network;

namespace Client.Classes
{
    public static class Routes
    {
        public static void FriendInfo(Header header, IClient client, byte[] data)
        {
            FriendList.AddFriend(new Friend(data.Cast<MSG_FRIEND_REQUEST>().GetUsername()));
        }
        public static void SaveToken(Header header, IClient client, byte[] data) 
            => Storage.SaveFile(new Common.FileSystem.mFile("token.t", data));
        
        public static void FriendRequest(Header header, IClient client, byte[] data)
        {
            Console.WriteLine($"{data.Cast<MSG_FRIEND_REQUEST>().GetUsername()} has sent a friend request!");
        }
        public static void OnFriendRequestResult(Header header, IClient client, byte[] data) {
            if(data.Length > sizeof(ushort)) {
                FriendList.AddFriend(new Friend(data.Cast<MSG_FRIEND_REQUEST>().GetUsername()));
                return;
            }
            
            ResultCodes resultCode = (ResultCodes)data.Cast<ushort>();
        }
    }
}
