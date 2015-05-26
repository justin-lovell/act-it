using System;
using System.Threading.Tasks;

namespace ActIt
{
    public static class StoryDirectAccessExtensions
    {
        public static void Direct(this StoryFactory storyFactory, Action<SceneActor> callbackAction)
        {
            var theSlugEvent = new object();

            var builder = storyFactory.CreateNestedBuilder();
            builder.Listen<object>((@event, actor) =>
            {
                if (ReferenceEquals(theSlugEvent, @event))
                {
                    callbackAction(actor);
                }
            });

            var story = builder.GenerateStory();
            story.Encounter(theSlugEvent);
        }

        public static Task DirectAsync(this StoryFactory storyFactory, Func<SceneActor, Task> callbackAction)
        {
            var theSlugEvent = new object();

            var builder = storyFactory.CreateNestedBuilder();
            builder.Listen<object>(
                (@event, actor) => ReferenceEquals(theSlugEvent, @event)
                                       ? callbackAction(actor)
                                       : null
                );

            var story = builder.GenerateStory();
            return story.EncounterAsync(theSlugEvent);
        }
    }
}