using MySqlConnector;

namespace Common.Database.Models
{
    public class User : IModel
    {
        private readonly uint id;
        public string username;
        public string password;
        public string address;
        public DateTime creationDate;
        public DateTime lastLogin;
        public ulong flags;

        internal User(MySqlDataReader reader)
        {
            id = reader.GetUInt32(0);
            username = reader.GetString(1);
            password = reader.GetString(2);
            address = reader.GetString(3);
            creationDate = reader.GetDateTime(4);
            lastLogin = reader.GetDateTime(5);
            flags = reader.GetUInt32(6);
        }
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
