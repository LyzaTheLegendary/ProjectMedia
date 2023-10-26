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
        public Query Where(string field, string value)
        {
            if (whereText != string.Empty)
                whereText += $"AND {field} = '{value}' ";
            else
                whereText +=  $"WHERE {field} = '{value}' ";
            return this;
        }
        public Query Update()
        {
            throw new NotImplementedException();
        }
        public Query Delete()
        {
            throw new NotImplementedException();
        }
        public override string ToString() // TODO implement update and delete
        {
            string queryStr = "";

            if(selectText != string.Empty)
                queryStr += $"SELECT {selectText} FROM {fromText} ";

            if (whereText != string.Empty)
                queryStr += $"{whereText}";
            return queryStr;
        }
        public static implicit operator string(Query query) => query.ToString();
    }
}
