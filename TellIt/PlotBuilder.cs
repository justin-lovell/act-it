using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TellIt
{
    public class PlotTapBuilder : IPlotTap
    {
        private readonly List<Listener> _listeners;

        public PlotTapBuilder()
            : this(new Listener[0])
        {
        }

        internal PlotTapBuilder(IEnumerable<Listener> listeners)
        {
            _listeners = listeners.ToList();
        }

        public StoryFactory GenerateStory()
        {
            return new StoryFactory(_listeners);
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
