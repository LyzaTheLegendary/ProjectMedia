using System.Text;

namespace Common.Utilities
{
    public static class StrUtil
    {
        public static byte[] ToUtf8Bytes(this string str) => Encoding.UTF8.GetBytes(str);
        public static byte[] ToAsciiBytes(this string str) => Encoding.ASCII.GetBytes(str);
        public static string ToUtf8Str(this byte[] bytes) => Encoding.UTF8.GetString(bytes);
        public static string ToAsciiStr(this byte[] bytes) => Encoding.ASCII.GetString(bytes);
        
    }
}
