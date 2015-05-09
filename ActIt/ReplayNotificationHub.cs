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
            return from ev in _eventsThatOccurred
                   let interestedEvent = ev as T
                   where interestedEvent != null
                   select interestedEvent;
        }
    }
}
