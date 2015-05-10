using System;
using System.Collections.Generic;

namespace ActIt
{
    internal sealed class StoryContext
    {
        private readonly Dictionary<Type, object> _contextInstances =
            new Dictionary<Type, object>();

        public T GetCurrentInstanceOrCreateInstance<T>(Func<T> createNewCallback)
        {
            object value;
            var wasFound = _contextInstances.TryGetValue(typeof(T), out value);

            if (wasFound)
            {
                return (T)value;
            }

            var newInstance = createNewCallback();
            _contextInstances.Add(typeof(T), newInstance);

            return newInstance;
        }

        public T GetCurrentInstanceOrCreateNew<T>() where T : new()
        {
            return GetCurrentInstanceOrCreateInstance(() => new T());
        }
    }
}