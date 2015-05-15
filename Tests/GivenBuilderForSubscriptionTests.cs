using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ActIt
{
    [TestFixture]
    public class GivenBuilderForSubscriptionTests
    {
        private class TheOtherEvent
        {
        }

        private class TheEvent
        {
        }

        [Test]
        public async Task WhenCreatingFactoryItShouldRespondToEvents()
        {
            // tracker
            var wasCalled = false;
            TheEvent theEventInstance = null;

            var theExpectedEventInstance = new TheEvent();

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheEvent>((@event, actor) =>
            {
                theEventInstance = @event;
                wasCalled = true;
            });


            // act
            var factory = builder.GenerateStory();
            await factory.EncounterAsync(theExpectedEventInstance);

            // assert
            Assert.That(wasCalled, Is.True);
            Assert.That(theEventInstance, Is.SameAs(theExpectedEventInstance));
        }

        [Test]
        public async Task WhenCreatingFactoryItShouldRespondToEventsAsync()
        {
            // tracker
            var wasCalled = false;
            TheEvent theEventInstance = null;

            var theExpectedEventInstance = new TheEvent();

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheEvent>(async (@event, actor) =>
            {
                theEventInstance = @event;
                await Task.Delay(20);
                wasCalled = true;
            });


            // act
            var factory = builder.GenerateStory();
            await factory.EncounterAsync(theExpectedEventInstance);

            // assert
            Assert.That(wasCalled, Is.True);
            Assert.That(theEventInstance, Is.SameAs(theExpectedEventInstance));
        }

        [Test]
        public async Task WhenListeningToEventsItShouldNotRespondToWhichItWasNotListeningTo()
        {
            // track
            var wasCalled = false;

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheEvent>((@event, actor) => wasCalled = true);

            // act
            var factory = builder.GenerateStory();
            await factory.EncounterAsync(new TheOtherEvent());

            // assert
            Assert.That(wasCalled, Is.False);
        }

        [Test]
        public async Task WhenListeningToEventsItShouldNotRespondToIrrelaventEvents()
        {
            // tracker
            var wasCalled = false;

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheOtherEvent>((@event, busSchedule) => wasCalled = true);

            // act
            var factory = builder.GenerateStory();
            await factory.EncounterAsync(new TheEvent());

            // assert
            Assert.That(wasCalled, Is.False);
        }

        [Test]
        public async Task WhenEavesDroppingItShouldTakeNoteOfAllEvents()
        {
            // tracker
            var event1 = new TheEvent();
            var event2 = new TheOtherEvent();

            var encounteredEvents = new List<object>();

            // arrange
            var builder = new PlotBuilder();
            builder.EavesDrop((@event, busSchedule) => encounteredEvents.Add(@event));

            // act
            var factory = builder.GenerateStory();
            await factory.EncounterAsync(event1);
            await factory.EncounterAsync(event2);

            // assert
            Assert.That(encounteredEvents.Count, Is.EqualTo(2));
            Assert.That(encounteredEvents, Has.Member(event1));
            Assert.That(encounteredEvents, Has.Member(event2));
        }

        [Test]
        public async Task WhenAsyncEavesDroppingItShouldTakeNoteOfAllEvents()
        {
            // tracker
            var event1 = new TheEvent();
            var event2 = new TheOtherEvent();

            var encounteredEvents = new List<object>();

            // arrange
            var builder = new PlotBuilder();
            builder.EavesDrop(async (@event, busSchedule) =>
            {
                encounteredEvents.Add(@event);
                await Task.Delay(1);
            });

            // act
            var factory = builder.GenerateStory();
            await factory.EncounterAsync(event1);
            await factory.EncounterAsync(event2);

            // assert
            Assert.That(encounteredEvents.Count, Is.EqualTo(2));
            Assert.That(encounteredEvents, Has.Member(event1));
            Assert.That(encounteredEvents, Has.Member(event2));
        }
    }
}
