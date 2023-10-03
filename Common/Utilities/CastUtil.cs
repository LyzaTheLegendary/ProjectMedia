using Network;

namespace Common.Utilities
{
    public static class CastUtil
    {
        public static T Cast<T>(this byte[] array)
            => MarshalUtil.BytesToStruct<T>(array);
        public static byte[] Cast(this Header header) 
            => MarshalUtil.StructToBytes(header);
    }
}
