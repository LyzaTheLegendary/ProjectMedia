namespace UpdateServer.Core.FileSystem
{
    public class Storage
    {
        private readonly string _rootPath;
        public Storage(string rootPath)
        {
            _rootPath = rootPath;
        }
        public byte[] GetUpdate(long updateNumber)
        {
            Directory.GetFiles
        }
    }
}
