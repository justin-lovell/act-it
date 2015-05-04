using System;
using System.Collections.Generic;

namespace TellIt
{
    public class PlotBuilder
    {
        private readonly List<Listener> _listeners = new List<Listener>();

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
            };
            _listeners.Add(listener);
        }
    }
}
