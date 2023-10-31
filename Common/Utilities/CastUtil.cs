using Network;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Common.Utilities
{
    public static class CastUtil
    {
        public static T Cast<T>(this byte[] array)
            => MarshalUtil.BytesToStruct<T>(array);
        public static byte[] Cast(this Header header) 
            => MarshalUtil.StructToBytes(header);

        public static T ReinterpretCast<T>(this ReadOnlyMemory<byte> array) where T : struct
            => Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(array.Span));
        
    }
}
