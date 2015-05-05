namespace TellIt
{
    public class SceneActor
    {
        private readonly StoryContext _context;

        internal SceneActor(StoryContext context)
        {
            _context = context;
        }

        public T Context<T>() where T : new()
        {
            return _context.GetCurrentInstanceOrCreateNew<T>();
        }
    }
}
