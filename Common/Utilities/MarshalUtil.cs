using System.Runtime.InteropServices;

namespace Common.Utilities
{
    public static class MarshalUtil // TODO this breaks somewhere?
    {
        public static byte[] StructToBytes<T>(T structure)
        {
            int structSize = Marshal.SizeOf(structure);
            byte[] bytes = new byte[structSize];

            IntPtr ptr = Marshal.AllocHGlobal(structSize);
            try
            {
                Marshal.StructureToPtr(structure!, ptr, false);
                Marshal.Copy(ptr, bytes, 0, structSize);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            return bytes;
        }

        public static T BytesToStruct<T>(byte[] buff)
        {
            int size = Marshal.SizeOf(typeof(T));
            if (buff.Length < size)
                throw new Exception("Invalid parameter");

            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(buff, 0, ptr, size);
                return (T)Marshal.PtrToStructure(ptr, typeof(T))!;
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
