using System.Threading.Tasks;

namespace ActIt
{
    public interface IInterruptController
    {
        Task<T> SingleResult<T>() where T : class;
    }
}