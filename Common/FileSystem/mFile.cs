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
        public string GetText() => Encoding.UTF8.GetString(_data);
        public int GetDataSize() => _data.Length;

        public static explicit operator byte[] (mFile file)
        {
            byte[] buff = new byte[file._data.Length + (file._fileName.Length + file._fileType.Length) * 2];
            using (MemoryStream ms = new(buff))
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(file._fileName);
                bw.Write(buff.Length);
                bw.Write(file._data);
            }

            
            return buff;
        }
        public static implicit operator mFile (byte[] buff) {
            using (MemoryStream ms = new(buff))
            using (BinaryReader br = new BinaryReader(ms))
            {
                string fileName = br.ReadString();
                int dataSize = br.ReadInt32();
                byte[] data = br.ReadBytes(dataSize);
                return new mFile(fileName, data);
            }
        }
    }
}
