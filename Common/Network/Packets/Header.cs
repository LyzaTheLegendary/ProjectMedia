using System.Runtime.InteropServices;

namespace Network
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Header
    {
        [MarshalAs(UnmanagedType.U2)]
        private readonly ushort _id;
        [MarshalAs(UnmanagedType.I4)]
        private readonly int _size;
        public Header(ushort id, int size)
        {
            _id = id;
            _size = size;
        }
        public int GetSize() => _size;
        public ushort GetId() => _id;
    }
}
