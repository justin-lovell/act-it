using System.Collections.Generic;
using System.Linq;

namespace TellIt
{
    public sealed class StoryFactory
    {
        private readonly StoryContext _context = new StoryContext();
        private readonly IEnumerable<Listener> _listeners;

        internal StoryFactory(IEnumerable<Listener> listeners)
        {
            _listeners = listeners.ToArray();
        }

        public void Encounter<TEvent>(TEvent theEvent)
        {
            var sceneActor = new SceneActor(_context);
            foreach (var listener in _listeners)
            {
                listener(theEvent, sceneActor);
            }
        }

        public PlotBuilder CreateNestedBuilder()
        {
            return new PlotBuilder(_listeners);
        }
    }
}
