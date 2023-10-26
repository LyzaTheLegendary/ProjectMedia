using Common.Database;
using Common.Database.Models;
using Common.Network;
using Common.Network.Packets.MediaServerPackets;
using MySqlConnector;
using System.Collections.Concurrent;

namespace Project_Friend_Media.Core.Users
{
    public static class UserManager
    {
        static readonly ConcurrentDictionary<int,User> _users = new();

        public static bool LoginUser(ID id, MSG_LOGIN loginInfo)
        {
            uint userId = 0;
            using(DbConn conn = DbConn.Factory())
            {
                MySqlDataReader reader = conn.ExecuteReader(new Query()
                    .Select("id")
                    .From("users")
                    .Where("username", loginInfo.GetUsername())
                    .Where("password", loginInfo.GetPassword()));
                
                if (reader.HasRows)
                {
                    reader.Read();
                    userId = reader.GetUInt32(0);
                }
                else
                    return false;
            }
            
            User user = User.GetUser(userId);
            // check flags for permissions or bans???

            if (_users.TryAdd((int)id, user))
                return false;
            
            return true;
        }

        public static void SaveUser(ID id, User user) => _users[id] = user; // Dangerious much? but don't care
        
        public static User GetUser(ID id)
        {
            if (!_users.TryGetValue(id, out User user))
                throw new Exception("Failed to get user!");

            return user;
        }
        public static void RemoveUser(ID id) => _users.TryRemove((int)id, out var _);

    }
}
