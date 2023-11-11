using System.Collections.Concurrent;

namespace Common.Network
{
    public class IDPool
    {
        private readonly ConcurrentQueue<int> availableIds = new ConcurrentQueue<int>();
        private int highestId = 1;
        private readonly object lockObject = new();

        public ID GetNewID()
        {
            if (!availableIds.TryDequeue(out int id))
            {
                lock (lockObject)
                {
                    id = highestId++;
                }
            }
            return new ID(id, ReturnToQueue);
        }

        private void ReturnToQueue(int id)
            => availableIds.Enqueue(id);
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
        public int GetNumber() => Id;
        ~ID()
        {
            if(_returnNumber == null) return;
            _returnNumber(Id);
        }
        public override int GetHashCode()
        {
            return Id;
        }
        public static implicit operator int(ID id)
        {
            return id.GetNumber();
        }
    }
}
