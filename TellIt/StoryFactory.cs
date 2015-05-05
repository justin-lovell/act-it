using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Task Encounter<TEvent>(TEvent theEvent)
        {
            var sceneActor = new SceneActor(_context);

            var tasks = from listener in _listeners
                        select listener(theEvent, sceneActor);

            return TaskEx.WhenAll(tasks);
        }

        public PlotBuilder CreateNestedBuilder()
        {
            return new PlotBuilder(_listeners);
        }
    }
}
