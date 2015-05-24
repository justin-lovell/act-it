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

        public T Context<T>(Func<T> createNewInstance)
        {
            return _context.GetCurrentInstanceOrCreateInstance(createNewInstance);
        }

        public Task InterruptAsync<TEvent>(TEvent theEvent, Action<ReplayNotificationHub> tapCallback)
        {
            if (tapCallback == null)
            {
                return InterruptAsync(theEvent);
            }

            var historicalEvents = new List<object>();
            var listeners = PipeEventsToHistoryRecounter(theEvent, historicalEvents);

            var nestedScene = new SceneActor(listeners, _context);

            Action<Task> continuationFunction = task =>
            {
                task.VerifyTaskNotFaulted();

                var replayHub = new ReplayNotificationHub(historicalEvents);
                tapCallback(replayHub);
            };

            return nestedScene.InterruptAsync(theEvent)
                              .ContinueWith(continuationFunction);
        }

        private IEnumerable<Listener> PipeEventsToHistoryRecounter<TEvent>(
            TEvent theEvent,
            List<object> eventsThatOccurred)
        {
            Listener temporaryListener = (@event, actor) =>
            {
                if (!ReferenceEquals(theEvent, @event))
                {
                    eventsThatOccurred.Add(@event);
                }

                return null;
            };
            var listeners = _listeners.Concat(new[] {temporaryListener});
            return listeners;
        }

        public Task InterruptAsync<TEvent>(TEvent theEvent)
        {
            var tasks = from listener in _listeners
                        select listener(theEvent, this);

            return TaskEx.WhenAll(tasks);
        }

        public IInterruptController InterruptAndControl<TEvent>(TEvent theEvent)
        {
            return new InterruptController<TEvent>(this, theEvent);
        }

        public void Interrupt<TEvent>(TEvent theEvent, Action<ReplayNotificationHub> tapCallback = null)
        {
            var task = InterruptAsync(theEvent, tapCallback);

            try
            {
                task.Wait();
            }
            catch (AggregateException aggregateException)
            {
                Exception lastException = null;

                aggregateException.Handle(exception =>
                {
                    lastException = exception;
                    return true;
                });

                if (lastException == null)
                {
                    throw;
                }

                var errorMessage =
                    "There was an error during the processing of the event. View inner exception for details";
                throw new InterruptExecutionException(errorMessage, lastException);
            }
        }
    }
}
