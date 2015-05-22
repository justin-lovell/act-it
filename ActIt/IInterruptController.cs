using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActIt
{
    public interface IInterruptController
    {
        IEnumerable<T> ObservingForEvent<T>() where T : class;
        Task<IEnumerable<T>> ObservingForEventAsync<T>() where T : class;
    }
}