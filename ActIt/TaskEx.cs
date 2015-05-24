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
            Task seed = IntoTaskResult<object>(null);

            Func<Task, Task, Task> aggregateFunc =
                (current, task) =>
                current.ContinueWith(_ => _.IsFaulted ? _ : task)
                       .Unwrap();

            return tasks.Where(_ => _ != null)
                        .Aggregate(seed, aggregateFunc);
        }

        public static void VerifyTaskNotFaulted(this Task task)
        {
            if (task.IsFaulted)
            {
                throw new InterruptExecutionException(
                    "There was an error during the observation. See inner exception for details.",
                    task.Exception);
            }
        }

        public static Task<TResult> IntoTaskResult<TResult>(this TResult result)
        {
            var taskCompletionSource = new TaskCompletionSource<TResult>();
            taskCompletionSource.SetResult(result);
            return taskCompletionSource.Task;
        }

    }
}
