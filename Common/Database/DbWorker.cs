using Common.Threading;
using MySqlConnector;
using System.Collections.Concurrent;

namespace Common.Database
{
    public static class DbWorker
    {
        private static TaskPool _pool;
        private static List<DbConn> _conn;
        private static BlockingCollection<MySqlCommand> queries = new();
        private static byte currIndex = 0;
        public static void Init(int workSize)
        {
            _conn = new List<DbConn>(workSize);
            _pool = new TaskPool(workSize + 1);

            for (int i = 0; i < workSize; i++)
                _conn.Add(DbConn.Factory());
            
            _pool.EnqueueTask(() =>
            {
                foreach(MySqlCommand query in queries.GetConsumingEnumerable())
                {
                    if (_conn.Count >= currIndex)
                        currIndex = 0;
                    _pool.EnqueueTask(() =>
                    {
                        if (_conn[currIndex++].ExecuteQuery(query) == 0)
                            Console.WriteLine($"Ran query: {query.CommandText} but 0 rows effected!");
                    });
                }
            });
        }
        public static void PendQuery(MySqlCommand query) => queries.Add(query);
    }
}
