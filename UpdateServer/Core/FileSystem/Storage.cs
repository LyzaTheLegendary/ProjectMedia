using Common.FileSystem;

namespace UpdateServer.Core.FileSystem
{
    public class Storage
    {
        private readonly string _rootPath;
        public Storage(string rootPath)
        {
            _rootPath = rootPath;
            if (!Directory.Exists(Path.Combine(_rootPath, "Updates")))
                Directory.CreateDirectory(Path.Combine(_rootPath, "Updates"));

        }
        public ulong GetLatestUpdateVer()
        {
            List<string> directories = Directory.GetFiles(Path.Combine(_rootPath, "Updates")).OrderBy(x => x.Split().Last()).ToList();
            if (directories.Count == 0)
                return 0;

            return ulong.Parse(directories[0]);
        }
        public mFile[] GetUpdate(ulong updateNumber)
        {
            string[] paths = Directory.GetFiles(Path.Combine(_rootPath,"Updates",updateNumber.ToString()));
            List<mFile> files = new(paths.Length);

            foreach(string path in paths)
                files.Add(new mFile(path));

            return files.ToArray();
        }
    }
}
