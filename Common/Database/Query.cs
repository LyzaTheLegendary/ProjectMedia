namespace Common.Database
{
    public class Query
    {
        private string selectText = "";
        private string fromText = "";
        private string whereText = "";
        public Query Select(string value) // if nothing is selected ( meaning nothing is returned we want to throw this on the Db worker pool!
        {
            selectText = value;
            return this;
        }
        public Query From(string value)
        {
            fromText = value;
            return this;
        }
        public Query Where(string value)
        {
            whereText = value;
            return this;
        }
        public override string ToString()
        {
            string queryStr = $"SELECT {selectText} FROM {fromText}";

            if (whereText != string.Empty)
                queryStr += $" WHERE {whereText}";
            return queryStr;
        }
        public static implicit operator string(Query query) => query.ToString();
    }
}
