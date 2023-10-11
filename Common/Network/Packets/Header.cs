using System.Runtime.InteropServices;

namespace Network
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Header
    {
        [MarshalAs(UnmanagedType.U2)]
        private readonly ushort _id;
        [MarshalAs(UnmanagedType.U4)]
        private readonly uint _size;
        public Header(ushort id, uint size)
        {
            _id = id;
            _size = size;
        }
        public uint GetSize() => _size;
        public ushort GetId() => _id;
    }
}
