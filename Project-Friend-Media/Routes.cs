using Common.Cryptography;
using Common.Database.Models;
using Common.Database;
using Common.Network.Clients;
using Common.Network.Packets.MediaServerPackets;
using Common.Network;
using Project_Friend_Media.Core.Users;
using Common.Utilities;
using Common.Network.Packets;
using MySqlConnector;
using Project_Friend_Media.Core.Users.Friends;
using Network;
using System.Text;

namespace Project_Friend_Media
{
    public static class Routes
    {
        public static void LoginUser(Header header, IClient client, byte[] data)
        {
            if (UserManager.LoginUser(client.GetId(), data.Cast<MSG_LOGIN>()))
            {
                ID id = client.GetId();
                User user = UserManager.GetUser(id)!;

                user.token = Md5Hash.CreateMD5String($"{id} + {user.username}"); ;
                user.Save(id);

                client.PendMessage((ushort)PacketIds.TOKEN,Encoding.ASCII.GetBytes(user.token));
                client.PendResult(header.GetResultId(), ResultCodes.Success);
            }
            else
                client.PendResult(header.GetResultId(), ResultCodes.NotFound);

        }
        public static void AddFriend(Header header, IClient client, byte[] data)
        {
            User? user = UserManager.GetUser(client.GetId());
            if (user == null)
            {
                client.PendResult(header.GetResultId(), ResultCodes.Denied);
                return;
            }

            ResultCodes result = FriendManager.FriendRequest(client, user, data.ToAsciiStr());

            client.PendResult(client.GetId(), result);
        }
        public static void OnHeartBeat(Header header, IClient client, byte[] data)
        {
            //mTime time = new mTime(BitConverter.ToInt64(data));
        }
    }
}
