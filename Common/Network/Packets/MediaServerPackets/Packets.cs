using Common.Utilities;
using System.Runtime.InteropServices;
using System.Text;

namespace Common.Network.Packets.MediaServerPackets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_HEARTBEAT
    {
        [MarshalAs(UnmanagedType.I8)]
        long binaryTime;
        public MSG_HEARTBEAT(mTime time) => binaryTime = time.GetBinaryTime();
    }
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
    public struct MSG_FRIEND_REQUEST
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        private readonly byte[] _username = new byte[50];
        public MSG_FRIEND_REQUEST(string username)
        {
            byte[] buff = Encoding.ASCII.GetBytes(username);

            if (buff.Length > _username.Length)
                throw new Exception("Username is too long!");

            Buffer.BlockCopy(buff, 0, _username, 0, buff.Length);
        }
        public string GetUsername() => Encoding.ASCII.GetString(_username).TrimEnd('\0');
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_FRIEND_REQUEST_ACCEPTED
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        private readonly byte[] _username = new byte[50];
        public MSG_FRIEND_REQUEST_ACCEPTED(string username)
        {
            byte[] buff = Encoding.ASCII.GetBytes(username);

            if (buff.Length > _username.Length)
                throw new Exception("Username is too long!");

            Buffer.BlockCopy(buff, 0, _username, 0, buff.Length);
        }
    }
}
