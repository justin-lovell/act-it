using System;
using System.Collections.Generic;

namespace TellIt
{
    public class SceneActor
    {
        private readonly IEnumerable<Listener> _listeners;

        internal SceneActor(IEnumerable<Listener> listeners)
        {
            _listeners = listeners;
        }

        public void Encounter<TEvent>(TEvent theEvent)
        {
            foreach (var listener in _listeners)
            {
                listener(theEvent, this);
            }
        }
    }
}