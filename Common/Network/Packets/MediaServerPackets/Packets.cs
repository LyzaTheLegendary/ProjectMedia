using System.Runtime.InteropServices;
using System.Text;

namespace Common.Network.Packets.MediaServerPackets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_LOGIN
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        private readonly byte[] _username = new byte[50];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] // Max md5 hash size
        private readonly byte[] _password = new byte[32];
        public MSG_LOGIN(string username, string password)
        {
            byte[] buff = Encoding.ASCII.GetBytes(username);

            if (buff.Length > _username.Length)
                throw new Exception("Username is too long!");

            Buffer.BlockCopy(buff, 0, _username, 0, buff.Length);
            buff = Encoding.ASCII.GetBytes(password);

            if (buff.Length > _password.Length)
                throw new Exception("Username is too long!");

            Buffer.BlockCopy(buff, 0, _password, 0, buff.Length);
        }
        public string GetUsername() => Encoding.ASCII.GetString(_username).TrimEnd('\0');
        public string GetPassword() => Encoding.ASCII.GetString(_password).TrimEnd('\0');
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_LOGIN_RESULT
    {
        [MarshalAs(UnmanagedType.U1)]
        private readonly byte _success;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        private readonly byte[] _reason = new byte[100];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] // Max md5 hash size
        private readonly byte[] _token = new byte[32];
        public MSG_LOGIN_RESULT(byte success, string token, string? _reason)
        {
            _success = success;
            byte[] buff = Encoding.ASCII.GetBytes(token);

            int size = buff.Length;
            if(buff.Length > _token.Length)
                size = _token.Length;

            Buffer.BlockCopy(buff,0, _token, 0, size);
            if (_reason == null)
                return;

            buff = Encoding.UTF8.GetBytes(_reason);
            size = buff.Length;

            if (buff.Length > _reason.Length)
                size = _reason.Length;

            Buffer.BlockCopy(buff, 0, _token, 0, size);
        }
    }
}
