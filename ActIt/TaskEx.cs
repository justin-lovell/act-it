using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActIt
{
    internal static class TaskEx
    {
        public static Task WhenAll(IEnumerable<Task> tasks)
        {
            Task currentTask = IntoTaskResult<object>(null);

            Func<Task, Task, Task> aggregateFunc =
                (current, task) =>
                current.ContinueWith(x => task, TaskContinuationOptions.OnlyOnRanToCompletion)
                       .Unwrap();

            return tasks.Where(_ => _ != null)
                        .Aggregate(currentTask, aggregateFunc);
        }

        public static Task<TResult> IntoTaskResult<TResult>(this TResult result)
        {
            var taskSource = new TaskCompletionSource<TResult>();
            taskSource.SetResult(result);
            return taskSource.Task;
        }
    }
}
