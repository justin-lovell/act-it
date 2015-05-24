using System;
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
        public void WhenEncounteringSynchronouslyWithAControllerItShouldReturnTheExpectedEvent()
        {
            var secondEvent = new TheSecondEvent();

            // arrange
            var plotBuilder = new PlotBuilder();

            plotBuilder.Listen<TheEvent>((@event, actor) => actor.Interrupt(secondEvent));

            // act
            var story = plotBuilder.GenerateStory();
            var result = story.EncounterAndControl(new TheEvent())
                              .ObservingForEvent<TheSecondEvent>()
                              .Single();

            // assert
            Assert.That(result, Is.SameAs(secondEvent));
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
            var result = (await story.EncounterAndControl(new TheEvent())
                                     .ObservingForEventAsync<TheSecondEvent>()).Single();

            // assert
            Assert.That(result, Is.SameAs(secondEvent));
        }

        [Test]
        [ExpectedException(typeof(InterruptExecutionException))]
        public void WhenInteruptingSynchronouslyThrowsAnExceptionItShouldThrowAnException()
        {
            // arrange
            var plotBuilder = new PlotBuilder();

            plotBuilder.Listen<TheEvent>((@event, actor) =>
                                         actor.InterruptAndControl(new TheSecondEvent())
                                              .ObservingForEvent<TheThirdEvent>()
                );
            plotBuilder.Listen<TheSecondEvent>(async (@event, actor) =>
            {
                await Task.Delay(19);
                throw new Exception("@event");
            });


            // act
            var story = plotBuilder.GenerateStory();
            story.Encounter(new TheEvent());

            // assert
        }

        [Test]
        public void WhenInteruptingSynchronouslyWithAnControllerItShouldReturnTheExpectedEvent()
        {
            TheThirdEvent receivedThirdEvent = null;
            var thirdEvent = new TheThirdEvent();

            // arrange
            var plotBuilder = new PlotBuilder();

            plotBuilder.Listen<TheEvent>((@event, actor) =>
                                         receivedThirdEvent = actor.InterruptAndControl(new TheSecondEvent())
                                                                   .ObservingForEvent<TheThirdEvent>()
                                                                   .Single()
                );
            plotBuilder.Listen<TheSecondEvent>((@event, actor) => actor.InterruptAsync(thirdEvent));


            // act
            var story = plotBuilder.GenerateStory();
            story.Encounter(new TheEvent());

            // assert
            Assert.That(receivedThirdEvent, Is.Not.Null);
            Assert.That(receivedThirdEvent, Is.SameAs(thirdEvent));
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
                                         (await actor.InterruptAndControl(new TheSecondEvent())
                                                     .ObservingForEventAsync<TheThirdEvent>()).Single()
                );
            plotBuilder.Listen<TheSecondEvent>((@event, actor) => actor.InterruptAsync(thirdEvent));


            // act
            var story = plotBuilder.GenerateStory();
            await story.EncounterAsync(new TheEvent());

            // assert
            Assert.That(receivedThirdEvent, Is.Not.Null);
            Assert.That(receivedThirdEvent, Is.SameAs(thirdEvent));
        }
    }
}
