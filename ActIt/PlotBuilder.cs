using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActIt
{
    public sealed class PlotBuilder
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

        public void Listen<T>(Action<T, SceneActor> callbackAction) where T : class
        {
            Listener listener = (@event, actor) =>
            {
                var o = @event as T;

                if (o != null)
                {
                    callbackAction(o, actor);
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

        public void EavesDrop(Action<object, SceneActor> callbackAction)
        {
            Listener listener = (@event, actor) =>
            {
                callbackAction(@event, actor);
                return TaskEx.IntoTaskResult<object>(null);
            };
            _listeners.Add(listener);
        }

        public void EavesDrop(Func<object, SceneActor, Task> callbackFunc)
        {
            Listener listener = (@event, actor) => callbackFunc(@event, actor);
            _listeners.Add(listener);
        }

    }
}
