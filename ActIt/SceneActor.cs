using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActIt
{
    public sealed class SceneActor
    {
        private readonly StoryContext _context;
        private readonly IEnumerable<Listener> _listeners;

        internal SceneActor(IEnumerable<Listener> listeners, StoryContext context)
        {
            _listeners = listeners;
            _context = context;
        }

        public T Context<T>() where T : new()
        {
            return _context.GetCurrentInstanceOrCreateNew<T>();
        }

        public Task Interrupt<TEvent>(TEvent theEvent, Action<ReplayNotificationHub> tapCallback)
        {
            if (tapCallback == null)
            {
                return Interrupt(theEvent);
            }

            var eventsThatOccurred = new List<object>();

            Listener temporaryListener = (@event, actor) =>
            {
                eventsThatOccurred.Add(@event);
                return TaskEx.IntoTaskResult<object>(null);
            };
            var listeners = _listeners.Concat(new[] {temporaryListener});

            return ExecuteInterruption(theEvent, listeners)
                .ContinueWith(task =>
                {
                    var replayHub = new ReplayNotificationHub(eventsThatOccurred);
                    tapCallback(replayHub);
                });
        }

        public Task Interrupt<TEvent>(TEvent theEvent)
        {
            return ExecuteInterruption(theEvent, _listeners);
        }

        private Task ExecuteInterruption<TEvent>(TEvent theEvent, IEnumerable<Listener> listeners)
        {
            var tasks = from listener in listeners
                        select listener(theEvent, this);

            return TaskEx.WhenAll(tasks);
        }
    }
}
