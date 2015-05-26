using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ActIt
{
    [TestFixture]
    public class GivenFactoryTests
    {
        private class TheEvent
        {
        }

        private class TheOtherEvent
        {
        }

        private class TheState
        {
        }

        [Test]
        public async Task WhenCreatingNestedBuilderItWillFirePreviousSubscriptions()
        {
            // track
            var wasCalled1 = false;

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheEvent>((@event, s) => wasCalled1 = true);

            var factory = builder.GenerateStory();

            var builder2 = factory.CreateNestedBuilder();
            var factory2 = builder2.GenerateStory();

            // act
            await factory2.EncounterAsync(new TheEvent());

            // assert
            Assert.That(wasCalled1, Is.True);
        }

        [Test]
        public void WhenCallingTheSynchronousTaskItShouldWork()
        {
            // track
            var wasCalled1 = false;

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheEvent>((@event, s) => wasCalled1 = true);

            var factory = builder.GenerateStory();

            // act
            factory.Encounter(new TheEvent());

            // assert
            Assert.That(wasCalled1, Is.True);
        }

        [Test]
        public void WhenThereAreMultipleEncountersItShouldNotShareTheState()
        {
            // track
            TheState firstState = null;
            TheState secondState = null;

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheEvent>((@event, s) => firstState = s.Context<TheState>());
            builder.Listen<TheOtherEvent>((@event, s) => secondState = s.Context<TheState>());

            var factory = builder.GenerateStory();

            // act
            factory.Encounter(new TheEvent());
            factory.Encounter(new TheOtherEvent());

            // assert
            Assert.That(firstState, Is.Not.Null);
            Assert.That(secondState, Is.Not.Null);
            Assert.That(firstState, Is.Not.SameAs(secondState));
        }

        [Test]
        public void WhenDirectoringItShouldAllowAccessToSceneActor()
        {
            // track
            var calls = new List<int>();

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheEvent>((@event, s) => calls.Add(2));

            // act
            var story = builder.GenerateStory();
            story.Direct(actor =>
            {
                calls.Add(1);
                story.Encounter(new TheEvent());
                calls.Add(3);
            });

            // assert
            Assert.That(calls, Has.Count.EqualTo(3));
            Assert.That(calls, Is.Ordered);
        }

        [Test]
        public async Task WhenDirectoringAsyncItShouldAllowAccessToSceneActor()
        {
            // track
            var calls = new List<int>();

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheEvent>((@event, s) => calls.Add(2));

            // act
            var story = builder.GenerateStory();
            await story.DirectAsync(async actor =>
            {
                calls.Add(1);
                await story.EncounterAsync(new TheEvent());
                calls.Add(3);
            });

            // assert
            Assert.That(calls, Has.Count.EqualTo(3));
            Assert.That(calls, Is.Ordered);
        }
    }
}
