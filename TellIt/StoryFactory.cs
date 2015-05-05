using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TellIt
{
    public sealed class StoryFactory
    {
        private readonly StoryContext _context;
        private readonly IEnumerable<Listener> _listeners;

        internal StoryFactory(IEnumerable<Listener> listeners, StoryContext context)
        {
            _context = context ?? new StoryContext();
            _listeners = listeners.ToArray();
        }

        public Task Encounter<TEvent>(TEvent theEvent)
        {
            var sceneActor = new SceneActor(_listeners, _context);
            return sceneActor.Interrupt(theEvent);
        }

        public PlotTapBuilder CreateNestedBuilder()
        {
            return new PlotTapBuilder(_listeners, _context);
        }
    }
}
