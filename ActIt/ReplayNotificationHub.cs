using System.Collections.Generic;
using System.Linq;

namespace ActIt
{
    public sealed class ReplayNotificationHub
    {
        private readonly List<object> _eventsThatOccurred;

        internal ReplayNotificationHub(List<object> eventsThatOccurred)
        {
            _eventsThatOccurred = eventsThatOccurred;
        }

        public IEnumerable<T> ReplayEvents<T>() where T : class
        {
            return _eventsThatOccurred.OfType<T>();
        }
    }
}
