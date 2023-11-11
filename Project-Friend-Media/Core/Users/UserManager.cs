using Common.Database;
using Common.Database.Models;
using Common.Network;
using Common.Network.Clients;
using Common.Network.Packets.MediaServerPackets;
using MySqlConnector;
using System.Collections.Concurrent;

namespace Project_Friend_Media.Core.Users
{
    //TODO: create a method to check if someone is online based on their Username instead of ID!
    public static class UserManager
    {
        static readonly ConcurrentDictionary<ID,User> _users = new();
        static Func<ID, IClient> _getClientViaId;
        
        public static void SetClientGetter(Func<ID, IClient> getClientViaId) => _getClientViaId = getClientViaId ;
        public static IClient? GetClientViaUsername(string username)
        { 
            //ID id = (from users in _users where users.Value.username == username select users.Key).First();
            ID[]? enumerable = (from users in _users where users.Value.username == username select users.Key).ToArray();

            if (enumerable.Count() == 0)
                return null;

            return _getClientViaId(enumerable.First());
        }
        public static bool LoginUser(ID id, MSG_LOGIN loginInfo)
        {
            
            uint userId = 0;
            string username = loginInfo.GetUsername();
            string password = loginInfo.GetPassword();

            using (DbConn conn = DbConn.Factory())
            using (MySqlCommand cmd = new())
            {
                cmd.CommandText = "SELECT id FROM users WHERE username = @username AND password = @password";
                cmd.Parameters.Add("@username", System.Data.DbType.String);
                cmd.Parameters.Add("@password", System.Data.DbType.String);
                cmd.Parameters["@username"].Value = username;
                cmd.Parameters["@password"].Value = password;

                MySqlDataReader reader = conn.ExecuteReader(cmd);

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

            if (!_users.TryAdd(id, user))
                return false;
            
            return true;
        }
        public static void SaveUser(ID id, User user) => _users[id] = user; // Dangerious much? but don't care
        public static User? GetUser(ID id)
        {
            _users.TryGetValue(id, out User? user);
                //throw new Exception("Failed to get user!");
            return user;
        }
        public static void RemoveUser(ID id) => _users.TryRemove(id, out var _);
    }
}
