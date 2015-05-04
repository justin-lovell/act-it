using NUnit.Framework;

namespace TellIt
{
    [TestFixture]
    public class GivenBuilderForFactoriesTests
    {
        private class TheEvent
        {
        }

        // todo: track instance data
        // todo: ensure that the events are raised in cascading style

        [Test]
        public void WhenCreatedTheListenersAreImmutableWhenInstanceWasCreated()
        {
            // track
            var wasCalled = false;

            // arrange
            var builder = new PlotBuilder();

            var factory = builder.GenerateStory();
            builder.Listen<TheEvent>((@event, busSchedule) => wasCalled = true);

            // act
            var schedule = factory.CreateSceneActor();
            schedule.Encounter(new TheEvent());

            // assert
            Assert.That(wasCalled, Is.False);
        }
    }
}
