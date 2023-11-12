using Common.Database;
using Common.Database.Models;
using Common.Network.Clients;
using Common.Network.Packets;
using Common.Network.Packets.MediaServerPackets;
using MySqlConnector;

namespace Project_Friend_Media.Core.Users.Friends
{
    public static class FriendManager
    {
        public static ResultCodes FriendRequest(IClient client, User user, string tUsername)
        {
            if (user.username == tUsername)
                return ResultCodes.Denied;

            IClient? tClient = UserManager.GetClientViaUsername(tUsername);
            User? tUser;

            if (tClient != null)
                tUser = UserManager.GetUser(tClient.GetId())!;
            else
                tUser = User.GetUser(tUsername);

            if(tUser == null)
            {
                //client.PendMessage((ushort)PacketIds.FRIEND_REQUEST_RESULT, (ushort)ResultCodes.NotFound);
                return ResultCodes.NotFound;
            }

            using (DbConn conn = DbConn.Factory())
            using(MySqlCommand cmd = new())
            {
                cmd.CommandText = "INSERT INTO friend_requests (`requester`,`target`) VALUES(@requester,@target);";
                cmd.Parameters.Add("@requester", System.Data.DbType.String);
                cmd.Parameters.Add("@target", System.Data.DbType.String);
                cmd.Parameters["@requester"].Value = user.username;
                cmd.Parameters["@target"].Value = tUser.username;

                try
                {
                    conn.ExecuteQuery(cmd);
                }
                catch(Exception){
                    //client.PendMessage((ushort)PacketIds.FRIEND_REQUEST_RESULT, (ushort)ResultCodes.AlreadyExist);
                    return ResultCodes.AlreadyExist;
                }
            }

            tClient?.PendMessage((ushort)PacketIds.FRIEND_REQUEST, new MSG_FRIEND_REQUEST(user.username));
            return ResultCodes.Success;
            //client.PendMessage((ushort)PacketIds.FRIEND_REQUEST_RESULT, new MSG_FRIEND_REQUEST(user.username));
        }
    }
}
