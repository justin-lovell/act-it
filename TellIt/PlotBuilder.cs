using System;
using System.Collections.Generic;
using System.Linq;

namespace TellIt
{
    public class PlotBuilder
    {
        private readonly List<Listener> _listeners;

        public PlotBuilder()
            : this(new Listener[0])
        {
        }

        internal PlotBuilder(IEnumerable<Listener> listeners)
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
    }
}
