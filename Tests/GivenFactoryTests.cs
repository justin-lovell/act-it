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
    }
}
