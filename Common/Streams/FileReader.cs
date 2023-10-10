using Common.FileSystem;
using System.Text;

namespace Common.Streams
{
    public class FileReader
    {
        MemoryStream _ms;
        public FileReader(byte[] data) { 
            _ms = new MemoryStream(data);
        }
        public mFile ReadFile()
        {
            byte strLen = (byte)_ms.ReadByte();
            byte[] buff = new byte[strLen];
            _ms.ReadExactly(buff);
            string fileName = Encoding.UTF8.GetString(buff);

            buff = new byte[sizeof(int)];
            _ms.Read(buff, 0, sizeof(int));
            int dataSize = BitConverter.ToInt32(buff);
            buff = new byte[dataSize];
            _ms.Read(buff);
            return new mFile(fileName, buff);
        }
    }
}
