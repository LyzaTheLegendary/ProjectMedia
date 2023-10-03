using System.Runtime.InteropServices;

namespace Common.Network.Packets.UpdateServerPackets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MSG_VERSION
    {
        [MarshalAs(UnmanagedType.U8)]
        public readonly ulong ver;
    }
}
