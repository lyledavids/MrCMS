using System;

namespace MrCMS.Tasks
{
    public interface IScheduledTaskRunner
    {
        void TriggerScheduledTasks();
        void ExecuteTask(Guid id);
    }
}