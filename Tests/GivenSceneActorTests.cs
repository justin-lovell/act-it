using System.Collections.Generic;
using NUnit.Framework;

namespace TellIt
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

        [Test]
        public void WhenRequestedContextItShouldReturnNewInstance()
        {
            TheContext theContext = null;

            // arrange
            var builder = new PlotBuilder();

            builder.Listen<TheEvent>((@event, busSchedule) =>
                                     theContext = busSchedule.Context<TheContext>());

            var story = builder.GenerateStory();

            // act
            story.Encounter(new TheEvent());

            // assert
            Assert.That(theContext, Is.Not.Null);
        }

        [Test]
        public void WhenRequestedContextTwiceItShouldReturnTheSameInstance()
        {
            var contexts = new List<TheContext>();

            // arrange
            var builder = new PlotBuilder();

            builder.Listen<TheEvent>((@event, busSchedule) =>
                                     contexts.Add(busSchedule.Context<TheContext>()));

            var story = builder.GenerateStory();

            // act
            story.Encounter(new TheEvent());
            story.Encounter(new TheEvent());

            // assert
            Assert.That(contexts.Count, Is.EqualTo(2));
            Assert.That(contexts[0], Is.SameAs(contexts[1]));
        }
    }
}
