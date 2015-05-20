using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActIt
{
    public sealed class StoryFactory
    {
        private readonly StoryContext _context = new StoryContext();
        private readonly IEnumerable<Listener> _listeners;

        internal StoryFactory(IEnumerable<Listener> listeners)
        {
            _listeners = listeners.ToArray();
        }

        public Task EncounterAsync<TEvent>(TEvent theEvent)
        {
            var sceneActor = CreateNewSceneActor();
            return sceneActor.InterruptAsync(theEvent);
        }

        private SceneActor CreateNewSceneActor()
        {
            return new SceneActor(_listeners, _context);
        }

        public PlotBuilder CreateNestedBuilder()
        {
            return new PlotBuilder(_listeners);
        }

        public IInterruptController EncounterAndControlAsync<TEvent>(TEvent theEvent)
        {
            var sceneActor = CreateNewSceneActor();
            return new InterruptController<TEvent>(sceneActor, theEvent);
        }
    }
}
