using Common.Network;
using MySqlConnector;
using Project_Friend_Media.Core.Users;

namespace Common.Database.Models
{
    public class User
    {
        private readonly uint id;
        public string username;
        public string password;
        public string address;
        public DateTime creationDate;
        public DateTime lastLogin;
        public ulong flags;
        public string token;
        public string language;
        public static User GetUser(uint id)
        {
            using (DbConn conn = DbConn.Factory()) {
                return new User(conn.ExecuteReader(
                    new Query().Select("*")
                    .From("users")
                    .Where("id",id.ToString())
                    ));
            }
        }
        internal User(MySqlDataReader reader)
        {
            reader.Read();
            id = reader.GetUInt32(0);
            username = reader.GetString(1);
            password = reader.GetString(2);
            address = reader.GetString(3);
            creationDate = reader.GetDateTime(4);
            lastLogin = reader.GetDateTime(5);
            flags = reader.GetUInt32(6);
            token = reader.GetString(7);
            language = reader.GetString(8);
        }
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Save(ID id)
        {
            string query = $"UPDATE users SET username = '{username}', password = '{password}', address = '{address}', creation_date = '{creationDate}', last_login_date = '{lastLogin}', flags = {flags}, token = '{token}', language = '{language}' WHERE id = {id}";
            DbWorker.PendQuery(query);
            UserManager.SaveUser(id,this);
        }
    }
}
