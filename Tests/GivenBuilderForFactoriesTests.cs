using NUnit.Framework;

namespace TellIt
{
    [TestFixture]
    public class GivenBuilderForFactoriesTests
    {
        private class TheEvent
        {
        }

        [Test]
        public void WhenCreatedTheListenersAreImmutableWhenInstanceWasCreated()
        {
            // track
            var wasCalled = false;

            // arrange
            var builder = new PlotBuilder();

            var story = builder.GenerateStory();
            builder.Listen<TheEvent>((@event, busSchedule) => wasCalled = true);

            // act
            story.Encounter(new TheEvent());

            // assert
            Assert.That(wasCalled, Is.False);
        }
    }
}
