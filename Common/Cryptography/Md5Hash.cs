using System.Text;

namespace Common.Cryptography
{
    public static class Md5Hash
    {
        public static string CreateMD5String(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Encoding.ASCII.GetString(hashBytes);
            }
        }
    }
}
