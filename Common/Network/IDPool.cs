namespace Common.Network
{
    public class IDPool
    {
        private int _highestNumber = 1;
        private readonly Queue<int> _availableNumbers = new();
        public void ReturnNumber(int id)
        {
            _availableNumbers.Enqueue(id);
        }
        public ID CreateID()
        {
            if (_availableNumbers.Count == 0)
                return new ID(_highestNumber++, ReturnNumber);
                
            return new ID(_availableNumbers.Dequeue(), ReturnNumber);
        }
    }
    public class ID
    {
        int Id;
        private readonly Action<int>? _returnNumber;
        public ID(int id, Action<int>? returnNumber = null) 
        { 
            _returnNumber = returnNumber; 
            Id = id; 
        }
        ~ID()
        {
            if(_returnNumber == null) return;
            _returnNumber(Id);
        }
    }
}
