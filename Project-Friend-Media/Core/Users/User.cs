using Common.Network;
using MySqlConnector;
using Project_Friend_Media.Core.Users;
using System.Data;

namespace Common.Database.Models
{
    public class User
    {
        private readonly uint _id;
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
            using (DbConn conn = DbConn.Factory())
            using (MySqlCommand cmd = new())
            {
                cmd.CommandText = "SELECT * FROM users WHERE id = @id";
                cmd.Parameters.Add("@id", DbType.UInt32);
                cmd.Parameters["@id"].Value = id;
                return new User(conn.ExecuteReader(cmd));
            }
        }
        public static User? GetUser(string username)
        {
            using (DbConn conn = DbConn.Factory())
            using (MySqlCommand cmd = new())
            {
                cmd.CommandText = "SELECT * FROM users WHERE username = @username";
                cmd.Parameters.Add("@username", DbType.String);
                cmd.Parameters["@username"].Value = username;
                MySqlDataReader reader = conn.ExecuteReader(cmd);
                if (!reader.HasRows)
                    return null;

                return new User(reader);
            }
        }
        internal User(MySqlDataReader reader)
        {
            reader.Read();
            _id = reader.GetUInt32(0);
            username = reader.GetString(1);
            password = reader.GetString(2);
            address = reader.GetString(3);
            creationDate = reader.GetDateTime(4);
            lastLogin = DateTime.Now;
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
            MySqlCommand cmd = new();
            cmd.CommandText = "UPDATE users SET username = @username, password = @password, address = @address, creation_date = @creation_date, last_login_date =  @last_login_date, flags = @flags, token = @token, language = @language WHERE id = @id";
            cmd.Parameters.Add("@username", DbType.String);
            cmd.Parameters.Add("@password", DbType.String);
            cmd.Parameters.Add("@address", DbType.String);
            cmd.Parameters.Add("@creation_date", DbType.String);
            cmd.Parameters.Add("@last_login_date", DbType.String);
            cmd.Parameters.Add("@flags", DbType.UInt64);
            cmd.Parameters.Add("@token", DbType.String);
            cmd.Parameters.Add("@language", DbType.String);
            cmd.Parameters.Add("@id", DbType.UInt32);

            cmd.Parameters["@username"].Value = username;
            cmd.Parameters["@password"].Value = password;
            cmd.Parameters["@address"].Value = address;
            cmd.Parameters["@creation_date"].Value = creationDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            cmd.Parameters["@last_login_date"].Value = lastLogin.ToString("yyyy-MM-dd HH:mm:ss.fff");
            cmd.Parameters["@flags"].Value = flags;
            cmd.Parameters["@language"].Value = language;
            cmd.Parameters["@token"].Value = token;
            cmd.Parameters["@id"].Value = _id;
            //string query = $"UPDATE users SET username = '{username}', password = '{password}', address = '{address}', creation_date = '{creationDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}', last_login_date = '{lastLogin.ToString("yyyy-MM-dd HH:mm:ss.fff")}', flags = {flags}, token = '{token}', language = '{language}' WHERE id = {_id}";
            DbWorker.PendQuery(cmd);
            UserManager.SaveUser(id,this);
        }
    }
}
