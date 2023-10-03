using System.Collections.Concurrent;

namespace Common.Threading
{
    public class TaskPool
    {
        private BlockingCollection<Action> taskQueue = new BlockingCollection<Action>();
        public TaskPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Task.Factory.StartNew(TaskLoop, TaskCreationOptions.LongRunning);
            }
        }

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
