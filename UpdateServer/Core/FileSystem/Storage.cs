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
        public int GetLatestUpdateVer()
        {
            string path = Path.Combine(_rootPath, "Updates");
            List<string> directories = Directory.GetDirectories(path).OrderBy(x => x.Split().Last()).ToList();
            if (directories.Count == 0)
                return 0;

            return directories.Count;
        }
        public mFile[]? GetUpdate(int updateNumber)
        {
            string path = Path.Combine(_rootPath, "Updates", updateNumber.ToString());
            if (!Directory.Exists(path))
                return null;

            string[] paths = Directory.GetFiles(path);
            List<mFile> files = new(paths.Length);

            foreach(string _path in paths)
                files.Add(new mFile(_path));

            return files.ToArray();
        }
    }
}
