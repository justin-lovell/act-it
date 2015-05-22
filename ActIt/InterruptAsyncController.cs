using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActIt
{
    internal class InterruptAsyncController<TEvent> : IInterruptAsyncController
    {
        private readonly SceneActor _sceneActor;
        private readonly TEvent _theEvent;

        public InterruptAsyncController(SceneActor sceneActor, TEvent theEvent)
        {
            _theEvent = theEvent;
            _sceneActor = sceneActor;
        }

        public Task<IEnumerable<T>> ObservingForEvents<T>() where T : class
        {
            IEnumerable<T> observingFor = null;

            Action<ReplayNotificationHub> tapCallback =
                hub => observingFor = hub.ReplayEvents<T>();

            Func<Task, IEnumerable<T>> continuationFunction = task =>
            {
                IEnumerable<T> enumerable =
                    observingFor as T[] ?? observingFor.ToArray();
                return enumerable;
            };

            return _sceneActor.InterruptAsync(_theEvent, tapCallback)
                              .ContinueWith(continuationFunction, TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}