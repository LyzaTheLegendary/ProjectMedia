using System.Collections.Concurrent;

namespace Common.Threading
{
    public class TaskPool
    {
        private BlockingCollection<Action> taskQueue = new BlockingCollection<Action>();
        private CancellationTokenSource tokenSource = new();
        public TaskPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Task.Factory.StartNew(TaskLoop,tokenSource.Token);
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
