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
        [MarshalAs(UnmanagedType.I4)]
        private readonly int _resultId; // Idea is to have a Concurrent dictonary with resultId , Action() to be ran whenever one that matches is ran with a ErrorType as body 
        public Header(ushort id, int size, int resultId = 0)
        {
            _id = id;
            _size = size;
            _resultId = resultId;
        }
        public int GetSize() => _size;
        public ushort GetId() => _id;
        public int GetResultId() => _resultId;
    }
}
