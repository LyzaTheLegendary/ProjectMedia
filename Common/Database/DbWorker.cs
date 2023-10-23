using Common.Threading;
using System.Collections.Concurrent;

namespace Common.Database
{
    public static class DbWorker
    {
        private static TaskPool _pool;
        private static List<DbConn> _conn;
        private static BlockingCollection<string> queries = new();
        private static byte currIndex = 0;
        public static void Init(int workSize)
        {
            _conn = new List<DbConn>(workSize);
            _pool = new TaskPool(workSize + 1);

            for (int i = 0; i < workSize; i++)
                _conn.Add(DbConn.Factory());
            
            _pool.EnqueueTask(() =>
            {
                foreach(string query in queries.GetConsumingEnumerable())
                {
                    if (query.Length >= currIndex)
                        currIndex = 0;
                    _pool.EnqueueTask(() =>
                    {
                        _conn[currIndex++].ExecuteQuery(query);
                    });
                }
            });
        }
        public static void PendQuery(string query) => queries.Add(query);
    }
}
