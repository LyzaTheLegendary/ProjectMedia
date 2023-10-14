using Common.FileSystem;
using Common.Network.Clients;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Common.Network.Packets.UpdateServerPackets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_VERSION
    {
        [MarshalAs(UnmanagedType.I4)]
        public readonly int ver;
        public MSG_VERSION(int ver) => this.ver = ver; 
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_UPDATE_STATUS
    {
        [MarshalAs(UnmanagedType.Bool)]
        public readonly bool updated;
        [MarshalAs(UnmanagedType.I4)]
        public readonly int files;
        [MarshalAs(UnmanagedType.I4)]
        public readonly int remoteVer;
        public MSG_UPDATE_STATUS(bool updated, int files, int remoteVer)
        {
            this.updated = updated;
            this.files = files;
            this.remoteVer = remoteVer;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_UPDATE_FINISHED
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public readonly byte[] address = new byte[50];
        [MarshalAs(UnmanagedType.U2)]
        public readonly ushort port;
        public MSG_UPDATE_FINISHED(Addr addr)
        {
            byte[] buff = Encoding.ASCII.GetBytes(addr.GetIP());
            Buffer.BlockCopy(buff, 0, address, 0, buff.Length);
            port = addr.GetPort();
        }
        public static explicit operator Addr(MSG_UPDATE_FINISHED packet)
        {
            //string address = new string(packet.address.Cast<char>().ToArray());
            string address = Encoding.ASCII.GetString(packet.address).TrimEnd('\0');//new string(new char[] { (char)packet.address[0], (char)packet.address[1], (char)packet.address[2], (char)packet.address[3] });
            return new Addr(address,packet.port);
        }
    }
}
