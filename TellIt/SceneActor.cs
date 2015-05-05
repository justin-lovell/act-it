using System;
using System.Threading.Tasks;

namespace TellIt
{
    public class SceneActor
    {
        private readonly StoryContext _context;
        private readonly Func<object, SceneActor, Task> _initiateEncounterFunc;

        internal SceneActor(StoryContext context, Func<object, SceneActor, Task> initiateEncounterFunc)
        {
            _context = context;
            _initiateEncounterFunc = initiateEncounterFunc;
        }

        public T Context<T>() where T : new()
        {
            return _context.GetCurrentInstanceOrCreateNew<T>();
        }

        public Task Interrupt<TEvent>(TEvent nestedEvent)
        {
            return _initiateEncounterFunc(nestedEvent, this);
        }
    }
}
