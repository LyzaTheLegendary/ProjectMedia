namespace Common.Utilities
{
    public class Settings
    {
        private static readonly Dictionary<string, string> _values = new();
        public static void Init(string filename)
        {
            string fileStr = File.ReadAllText(filename);
            foreach(string line in fileStr.Split("\r\n"))
            {
                if (line.StartsWith("//") || line.StartsWith("\r\n"))
                    continue;

                string[] lineParams = line.Split('=');
                _values.Add(lineParams[0].Trim(), lineParams[1].Trim());
            }
        }
        public static string GetValue(string key)
        {
            if (!_values.TryGetValue(key, out string? value))
                throw new Exception($"{key} does not exist!");

            return value;
        }
    }
}
