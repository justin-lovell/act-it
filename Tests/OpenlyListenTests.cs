using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ActIt
{
    [TestFixture]
    public class OpenlyListenTests
    {
        private class TheOtherEvent
        {
        }

        private class TheEvent
        {
        }

        private class OpenEventAsyncHandler<T> : IOpenPlotListenerAsyncHandler<OpenEvent<T>>
        {
            private static async Task HandleAysnc(T theEvent, SceneActor actor)
            {
                var newEvent = new HandledEvent(theEvent);
                await actor.InterruptAsync(newEvent);
            }

            public Task Handle(OpenEvent<T> theEvent, SceneActor actor)
            {
                return HandleAysnc(theEvent.WrappedEvent, actor);
            }
        }

        private class HandledEvent
        {
            public HandledEvent(object eventInstance)
            {
                EventInstance = eventInstance;
            }

            public object EventInstance { get; private set; }
        }

        private class OpenEvent<T>
        {
            public OpenEvent(T wrappedEvent)
            {
                WrappedEvent = wrappedEvent;
            }

            public T WrappedEvent { get; private set; }
        }

        [Test]
        public async Task ItShouldTakeNoteOfPartiallyInterestingEvents()
        {
            // tracker
            var event1 = new TheEvent();
            var event2 = new TheOtherEvent();

            var encounteredEvents = new List<object>();

            // arrange
            var builder = new PlotBuilder();
            builder.OpenlyAsyncListen(typeof (OpenEventAsyncHandler<>));
            builder.Listen<HandledEvent>((@event, actor) => encounteredEvents.Add(@event.EventInstance));

            // act
            var factory = builder.GenerateStory();
            await factory.EncounterAsync(new OpenEvent<TheEvent>(event1));
            await factory.EncounterAsync(new OpenEvent<TheOtherEvent>(event2));
            await factory.EncounterAsync(new TheEvent());

            // assert
            Assert.That(encounteredEvents.Count, Is.EqualTo(2));
            Assert.That(encounteredEvents, Has.Member(event1));
            Assert.That(encounteredEvents, Has.Member(event2));
        }
    }
}
