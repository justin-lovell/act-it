using System.Threading.Tasks;
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
        public async Task WhenCreatedTheListenersAreImmutableWhenInstanceWasCreated()
        {
            // track
            var wasCalled = false;

            // arrange
            var builder = new PlotTapBuilder();

            var story = builder.GenerateStory();
            builder.Listen<TheEvent>((@event, busSchedule) => wasCalled = true);

            // act
            await story.Encounter(new TheEvent());

            // assert
            Assert.That(wasCalled, Is.False);
        }
    }
}
