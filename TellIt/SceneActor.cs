using System;
using System.Collections.Generic;

namespace TellIt
{
    public class SceneActor
    {
        private readonly Dictionary<Type, object> _contextInstances =
            new Dictionary<Type, object>();

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

        public T Context<T>() where T : new()
        {
            object value;
            var wasFound = _contextInstances.TryGetValue(typeof (T), out value);

            if (wasFound)
            {
                return (T) value;
            }

            var newInstance = new T();
            _contextInstances.Add(typeof (T), newInstance);

            return newInstance;
        }
    }
}
