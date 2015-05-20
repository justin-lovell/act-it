using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ActIt
{
    [TestFixture]
    public class GivenSceneInterruptControllerTests
    {
        private class TheEvent
        {
        }

        private class TheSecondEvent
        {
        }

        private class TheThirdEvent
        {
        }

        [Test]
        public async Task WhenEncounteringWithAControllerItShouldReturnTheExpectedEvent()
        {
            var secondEvent = new TheSecondEvent();

            // arrange
            var plotBuilder = new PlotBuilder();

            plotBuilder.Listen<TheEvent>((@event, actor) => actor.InterruptAsync(secondEvent));

            // act
            var story = plotBuilder.GenerateStory();
            var result = (await story.EncounterAndControlAsync(new TheEvent())
                                     .ObservingForEvents<TheSecondEvent>()).Single();

            // assert
            Assert.That(result, Is.SameAs(secondEvent));
        }

        [Test]
        public async Task WhenInteruptingWithAnControllerItShouldReturnTheExpectedEvent()
        {
            TheThirdEvent receivedThirdEvent = null;
            var thirdEvent = new TheThirdEvent();

            // arrange
            var plotBuilder = new PlotBuilder();

            plotBuilder.Listen<TheEvent>(async (@event, actor) =>
                                         receivedThirdEvent =
                                         (await actor.InterruptAndControlAsync(new TheSecondEvent())
                                                     .ObservingForEvents<TheThirdEvent>()).Single()
                );
            plotBuilder.Listen<TheSecondEvent>((@event, actor) => actor.InterruptAsync(thirdEvent));


            // act
            var story = plotBuilder.GenerateStory();
            await story.EncounterAsync(new TheEvent());

            // assert
            Assert.That(thirdEvent, Is.Not.Null);
            Assert.That(receivedThirdEvent, Is.SameAs(thirdEvent));
        }
    }
}
