
using MySqlConnector;

namespace Common.Database
{
    //TODO: use this to create DB models
    public class DbConn : IDisposable
    {
        private readonly MySqlConnection conn;
        #region connection details
        private static string? _host;
        private static string? _user;
        private static string? _password;
        private static string? _database;
        #endregion
        public static void Init(string host, string user, string password, string database)
        {
            _host = host;
            _user = user;
            _password = password;
            _database = database;
        }
        public static DbConn Factory()
        {
            if (_host == null)
                throw new Exception("Db was not intialized");

            return new DbConn(_host, _user!, _password!, _database!);
        }
        internal DbConn(string host, string user, string password, string Dbname)
        {
            string connStr = string.Format("server={0};uid={1};pwd={2};database={3}", host,user,password,Dbname);
            conn = new MySqlConnection(connStr);
            conn.Open();
        }
        public int ExecuteQuery(string query)
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            using(MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                return cmd.ExecuteNonQuery();
            }
        }
        public MySqlDataReader ExecuteReader(string query)
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                return cmd.ExecuteReader();
            }
        }
        public void Dispose()
        {
            conn.Close();   
        }
    }
}
