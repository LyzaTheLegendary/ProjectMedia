using System.Collections.Concurrent;

namespace Common.Threading
{
    public class TaskPool
    {
        private BlockingCollection<Action> taskQueue = new();
        private CancellationTokenSource tokenSource = new();
        private int currentSize;
        public TaskPool(int count)
        {
            currentSize = count;
            Start(count);
        }
        private void Start(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Task.Factory.StartNew(TaskLoop, tokenSource.Token);
            }
        }
        public void Resize(int count)
        {
            for (; currentSize < count; currentSize++)
            {
                Task.Factory.StartNew(TaskLoop, tokenSource.Token);
            }
        }
        public void Stop() => tokenSource.Cancel();
        public bool IsBusy() => taskQueue.Count > 0;
        public void EnqueueTask(Action task)
        {
            
            taskQueue.Add(task);
        }
        private void TaskLoop()
        {
            foreach (Action task in taskQueue.GetConsumingEnumerable())
            {
                task?.Invoke();
            }
        }
    }
}
