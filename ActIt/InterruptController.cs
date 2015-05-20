using System;
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

        public Task<T> SingleResult<T>() where T : class
        {
            var capturedResult = default(T);

            Action<ReplayNotificationHub> tapCallback =
                hub => capturedResult = hub.ReplayEvents<T>().Single();

            return _sceneActor.InterruptAsync(_theEvent, tapCallback)
                              .ContinueWith(task => capturedResult);
        }
    }
}