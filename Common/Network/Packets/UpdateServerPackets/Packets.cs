using Common.FileSystem;
using Common.Network.Clients;
using System.Net;
using System.Runtime.InteropServices;

namespace Common.Network.Packets.UpdateServerPackets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_VERSION
    {
        [MarshalAs(UnmanagedType.U8)]
        public readonly ulong ver;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_UPDATE_STATUS
    {
        [MarshalAs(UnmanagedType.Bool)]
        public readonly bool updated;
        [MarshalAs(UnmanagedType.U4)]
        public readonly uint files;

        public MSG_UPDATE_STATUS(bool updated, uint files)
        {
            this.updated = updated;
            this.files = files;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_UPDATE_FINISHED
    {
        [MarshalAs(UnmanagedType.U1, SizeConst = 4)]
        public readonly byte[] address;
        [MarshalAs(UnmanagedType.U2)]
        public readonly ushort port;
        public MSG_UPDATE_FINISHED(Addr addr)
        {
            address = addr.GetIP().Split('.').Select(byte.Parse).ToArray();
            port = addr.GetPort();
        }
        public static explicit operator Addr(MSG_UPDATE_FINISHED packet)
        {
            string address = new string(packet.address.Cast<char>().ToArray());
            return new Addr(address,packet.port);
        }
    }
}
