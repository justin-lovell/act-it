using System;
using System.Collections.Generic;

namespace ActIt
{
    internal sealed class StoryContext
    {
        private readonly Dictionary<Type, object> _contextInstances =
            new Dictionary<Type, object>();

        public T GetCurrentInstanceOrCreateNew<T>() where T : new()
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