using System.Runtime.InteropServices;

namespace Common.Utilities
{
    public static class MarshalUtil
    {
        public static byte[] StructToBytes<T>(T structure)
        {
            int size = Marshal.SizeOf(structure);
            nint ptr = Marshal.AllocCoTaskMem(size);

            byte[] buff = new byte[size];
            Marshal.Copy(ptr, buff, 0, size);
            Marshal.FreeCoTaskMem(ptr);

            return buff;
        }

        public static T BytesToStruct<T>(byte[] buff)
        {
            int size = Marshal.SizeOf<T>();

            if (buff.Length != size)
                throw new ArgumentException("Buffer provided is not the same size as the unmanaged size of struct given!");

            nint ptr = Marshal.AllocCoTaskMem(size);
            Marshal.Copy(buff,0, ptr, size);

            T? structure = Marshal.PtrToStructure<T>(ptr) ?? throw new Exception("Failed to turn buff from ptr into structure!");
            Marshal.FreeCoTaskMem(ptr);

            return structure;
        }
    }
}
