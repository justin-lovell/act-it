using System;
using System.Collections.Generic;
using System.Linq;

namespace TellIt
{
    public sealed class StoryFactory
    {
        private readonly IEnumerable<Listener> _listeners;

        internal StoryFactory(IEnumerable<Listener> listeners)
        {
            _listeners = listeners.AsEnumerable();
        }

        public SceneActor CreateSceneActor()
        {
            return new SceneActor(_listeners);
        }

        public PlotBuilder CreateNestedBuilder()
        {
            throw new NotImplementedException();
        }
    }
}