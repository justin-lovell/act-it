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
        public void WhenCreatingFactoryItShouldRespondToEvents()
        {
            // tracker
            var wasCalled = false;
            TheEvent theEventInstance = null;

            var theExpectedEventInstance = new TheEvent();

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheEvent>((@event, busSchedule) =>
            {
                theEventInstance = @event;
                wasCalled = true;
            });


            // act
            var factory = builder.GenerateStory();
            var schedule = factory.CreateNewSchedule();

            schedule.Encounter(theExpectedEventInstance);

            // assert
            Assert.That(wasCalled, Is.True);
            Assert.That(theEventInstance, Is.SameAs(theExpectedEventInstance));
        }

        [Test]
        public void WhenListeningToEventsItShouldNotRespondToIrrelaventEvents()
        {
            // track
            var wasCalled = false;

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheEvent>((@event, busSchedule) => wasCalled = true);

            // act
            var factory = builder.GenerateStory();
            var schedule = factory.CreateNewSchedule();

            schedule.Encounter(new TheOtherEvent());

            // assert
            Assert.That(wasCalled, Is.False);
        }

        [Test]
        public void WhenListeningToEventsItShouldNotRespondToIrrelaventEvents2()
        {
            // tracker
            var wasCalled = false;

            // arrange
            var builder = new PlotBuilder();
            builder.Listen<TheOtherEvent>((@event, busSchedule) => wasCalled = true);

            // act
            var factory = builder.GenerateStory();
            var schedule = factory.CreateNewSchedule();

            schedule.Encounter(new TheEvent());

            // assert
            Assert.That(wasCalled, Is.False);
        }
    }
}
