using System.Threading.Tasks;
using NUnit.Framework;

namespace TellIt
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
            await factory.Encounter(theExpectedEventInstance);

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
            await factory.Encounter(theExpectedEventInstance);

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
            await factory.Encounter(new TheOtherEvent());

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
            await factory.Encounter(new TheEvent());

            // assert
            Assert.That(wasCalled, Is.False);
        }
    }
}
