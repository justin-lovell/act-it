using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActIt
{
    public interface IInterruptController
    {
        Task<IEnumerable<T>> ObservingForEvents<T>() where T : class;
    }
}