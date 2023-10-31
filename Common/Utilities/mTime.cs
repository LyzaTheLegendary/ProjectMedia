namespace Common.Utilities
{
    public class mTime
    {
        private readonly DateTime _time;
        public mTime(DateTime time) 
            => _time = time;
        
        public mTime(long binaryTime) 
            => _time = new DateTime(binaryTime);
        
        public override string ToString() 
            => _time.ToString("F");
        public long GetBinaryTime() => _time.ToBinary();
    }
}
