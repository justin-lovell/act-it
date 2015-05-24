using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActIt
{
    internal sealed class InterruptController<TEvent> : IInterruptController
    {
        private readonly SceneActor _sceneActor;
        private readonly TEvent _theEvent;

        public InterruptController(SceneActor sceneActor, TEvent theEvent)
        {
            _theEvent = theEvent;
            _sceneActor = sceneActor;
        }

        Task<IEnumerable<T>> IInterruptController.ObservingForEventAsync<T>()
        {
            IEnumerable<T> observingFor = null;

            Action<ReplayNotificationHub> tapCallback =
                hub => observingFor = hub.ReplayEvents<T>();

            Func<Task, IEnumerable<T>> continuationFunction = task =>
            {
                task.VerifyTaskNotFaulted();

                IEnumerable<T> enumerable =
                    observingFor as T[] ?? observingFor.ToArray();
                return enumerable;
            };

            return _sceneActor.InterruptAsync(_theEvent, tapCallback)
                              .ContinueWith(continuationFunction);
        }

        IEnumerable<T> IInterruptController.ObservingForEvent<T>()
        {
            IEnumerable<T> observingFor = null;

            Action<ReplayNotificationHub> tapCallback =
                hub => observingFor = hub.ReplayEvents<T>();

            _sceneActor.Interrupt(_theEvent, tapCallback);

            return observingFor;
        }
    }
}