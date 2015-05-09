using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ActIt
{
    [TestFixture]
    public class GivenSceneActorTests
    {
        private class TheEvent
        {
        }

        private class TheContext
        {
        }

        public class TheSecondEvent
        {
        }

        public class TheThirdEvent
        {
        }

        [Test]
        public async Task WhenCallingNestedInterruptItShouldCascadeInCallingOrder()
        {
            var checkpoints = new List<int>();

            // arrange
            var builder = new PlotBuilder();

            builder.Listen<TheEvent>(async (@event, actor) =>
            {
                checkpoints.Add(1);
                await actor.InterruptAsync(new TheSecondEvent());
                checkpoints.Add(3);
            });
            builder.Listen<TheSecondEvent>((@event, actor) => checkpoints.Add(2));

            // act
            var story = builder.GenerateStory();
            await story.EncounterAsync(new TheEvent());

            // assert
            Assert.That(checkpoints.Count, Is.EqualTo(3));
            Assert.That(checkpoints, Is.Ordered);
        }

        [Test]
        public async Task WhenCallingNestedInterruptWithListenerItShouldNotListenToOwnEcho()
        {
            var wasOwnEchoHeard = false;

            // arrange
            var builder = new PlotBuilder();

            builder.Listen<TheEvent>((@event, actor) =>
            {
                actor.InterruptAsync(new TheSecondEvent(),
                                tap => wasOwnEchoHeard = tap.ReplayEvents<TheSecondEvent>().Any());
            });

            // act
            var story = builder.GenerateStory();
            await story.EncounterAsync(new TheEvent());

            // assert
            Assert.That(wasOwnEchoHeard, Is.False);
        }

        [Test]
        public async Task WhenCallingNestedInterruptWithListenersItShouldCascadeInRegistrationOrder()
        {
            var checkpoints = new List<int>();

            // arrange
            var builder = new PlotBuilder();

            builder.Listen<TheEvent>(async (@event, actor) =>
            {
                checkpoints.Add(1);
                await actor.InterruptAsync(new TheSecondEvent(),
                                tap =>
                                {
                                    if (tap.ReplayEvents<TheThirdEvent>().Count() == 1)
                                    {
                                        checkpoints.Add(3);
                                    }
                                });
                checkpoints.Add(4);
            });
            builder.Listen<TheSecondEvent>(async (@event, actor) =>
            {
                await actor.InterruptAsync(new TheThirdEvent());
                checkpoints.Add(2);
            });

            // act
            var story = builder.GenerateStory();
            await story.EncounterAsync(new TheEvent());

            // assert
            Assert.That(checkpoints.Count, Is.EqualTo(4));
            Assert.That(checkpoints, Is.Ordered);
        }

        [Test]
        public async Task WhenRequestedContextItShouldReturnNewInstance()
        {
            TheContext theContext = null;

            // arrange
            var builder = new PlotBuilder();

            builder.Listen<TheEvent>((@event, busSchedule) =>
                                     theContext = busSchedule.Context<TheContext>());

            var story = builder.GenerateStory();

            // act
            await story.EncounterAsync(new TheEvent());

            // assert
            Assert.That(theContext, Is.Not.Null);
        }

        [Test]
        public async Task WhenRequestedContextTwiceItShouldReturnTheSameInstance()
        {
            var contexts = new List<TheContext>();

            // arrange
            var builder = new PlotBuilder();

            builder.Listen<TheEvent>((@event, busSchedule) =>
                                     contexts.Add(busSchedule.Context<TheContext>()));

            var story = builder.GenerateStory();

            // act
            await story.EncounterAsync(new TheEvent());
            await story.EncounterAsync(new TheEvent());

            // assert
            Assert.That(contexts.Count, Is.EqualTo(2));
            Assert.That(contexts[0], Is.SameAs(contexts[1]));
        }
    }
}
