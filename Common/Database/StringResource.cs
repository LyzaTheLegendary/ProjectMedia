using MySqlConnector;

namespace Common.Database
{
    public static class StringResource
    {
        public static string GetString(string language, uint id)
        {
            using (DbConn conn = DbConn.Factory())
            {
                MySqlDataReader reader = conn.ExecuteReader($"SELECT text FROM stringresource_{language} WHERE id = {id}");
                reader.Read();
                return reader.GetString(0);
            }
        }
    }
}
