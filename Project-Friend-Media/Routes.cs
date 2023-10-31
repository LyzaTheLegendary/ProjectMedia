using Common.Cryptography;
using Common.Database.Models;
using Common.Database;
using Common.Network.Clients;
using Common.Network.Packets.MediaServerPackets;
using Common.Network;
using Project_Friend_Media.Core.Users;
using Common.Utilities;

namespace Project_Friend_Media
{
    public static class Routes
    {
        public static void LoginUser(IClient client, byte[] data)
        {
            MSG_LOGIN_RESULT result;
            if (UserManager.LoginUser(client.GetId(), data.Cast<MSG_LOGIN>()))
            {
                ID id = client.GetId();
                User user = UserManager.GetUser(id);
                string token = Md5Hash.CreateMD5String($"{id} + {user.username}");
                result = new MSG_LOGIN_RESULT(1, token, StringResource.GetString(user.language, 1));
                user.token = token;
                user.Save(id);

                client.PendMessage((ushort)PacketIds.LOGIN_RESULT, result);
            }
            else
            {
                result = new MSG_LOGIN_RESULT(0, "", StringResource.GetString("EN", 2));
                client.PendMessage((ushort)PacketIds.LOGIN_RESULT, result);
            }
        }
        public static void OnHeartBeat(IClient client, byte[] data)
        {
            //mTime time = new mTime(BitConverter.ToInt64(data));
        }
    }
}
