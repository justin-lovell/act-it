using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActIt
{
    internal class InterruptController<TEvent> : IInterruptController
    {
        private readonly SceneActor _sceneActor;
        private readonly TEvent _theEvent;

        public InterruptController(SceneActor sceneActor, TEvent theEvent)
        {
            _theEvent = theEvent;
            _sceneActor = sceneActor;
        }

        public Task<IEnumerable<T>> ObservingForEvents<T>() where T : class
        {
            IEnumerable<T> observingFor = null;

            Action<ReplayNotificationHub> tapCallback =
                hub => observingFor = hub.ReplayEvents<T>();

            return _sceneActor.InterruptAsync(_theEvent, tapCallback)
                              .ContinueWith(task =>
                              {
                                  IEnumerable<T> enumerable =
                                      observingFor as T[] ?? observingFor.ToArray();
                                  return enumerable;
                              });
        }
    }
}