using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActIt
{
    public class PlotBuilder
    {
        private readonly List<Listener> _listeners;
        private readonly StoryContext _context;

        public PlotBuilder()
            : this(new Listener[0], new StoryContext())
        {
        }

        internal PlotBuilder(IEnumerable<Listener> listeners, StoryContext context)
        {
            _context = context;
            _listeners = listeners.ToList();
        }

        public StoryFactory GenerateStory()
        {
            return new StoryFactory(_listeners, _context);
        }

        public void Listen<T>(Action<T, SceneActor> callbackFunc) where T : class
        {
            Listener listener = (@event, actor) =>
            {
                var o = @event as T;

                if (o != null)
                {
                    callbackFunc(o, actor);
                }

                return TaskEx.IntoTaskResult<object>(null);
            };
            _listeners.Add(listener);
        }

        public void Listen<T>(Func<T, SceneActor, Task> callbackFunc) where T : class
        {
            Listener listener = (@event, actor) =>
            {
                var o = @event as T;

                return o != null
                           ? callbackFunc(o, actor)
                           : TaskEx.IntoTaskResult<object>(null);
            };
            _listeners.Add(listener);
        }
    }
}
