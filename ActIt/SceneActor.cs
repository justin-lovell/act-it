using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActIt
{
    public sealed class SceneActor
    {
        private readonly IEnumerable<Listener> _listeners;
        private readonly StoryContext _context;

        internal SceneActor(IEnumerable<Listener> listeners, StoryContext context)
        {
            _listeners = listeners;
            _context = context;
        }

        public T Context<T>() where T : new()
        {
            return _context.GetCurrentInstanceOrCreateNew<T>();
        }

        public Task Interrupt<TEvent>(TEvent theEvent, Action<IPlotTap> tapCallback)
        {
            // all of these API calls need to be taken away

            var nestedPlotBuilder = new PlotBuilder(_listeners, _context);

            tapCallback(nestedPlotBuilder);

            var nestedStory = nestedPlotBuilder.GenerateStory();
            return nestedStory.Encounter(theEvent);
        }

        public Task Interrupt<TEvent>(TEvent theEvent)
        {
            var tasks = from listener in _listeners
                        select listener(theEvent, this);

            return TaskEx.WhenAll(tasks);
        }
    }
}
