using System.Text;

namespace Common.FileSystem
{
    public struct mFile
    {
        readonly string? _path = null;
        readonly string _fileName;
        readonly string _fileType;
        readonly byte[] _data;
        public mFile(string path)
        {
            _path = path;
            _fileName = path.Split("\\").Last();
            _data = File.ReadAllBytes(path);
            _fileType = path.Split('.').Last();
        }
        public mFile(string fileName, byte[] buff) // have to read Header to check file type!
        {
            _data = buff;
            _fileName = fileName;
            _fileType = fileName.Split(".")[1] ?? "";
        }
        public string GetFileName() => _fileName;
        public string? GetPath() => _path;
        public string GetFileType() => _fileType;
        public byte[] GetData() => _data;

        public static explicit operator byte[] (mFile file)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(file.GetFileName().Length));
            bytes.AddRange(Encoding.UTF8.GetBytes(file.GetFileName()));
            bytes.AddRange(BitConverter.GetBytes(file._data.Length));
            bytes.AddRange(file._data);
            return bytes.ToArray();
        }
    }
}
