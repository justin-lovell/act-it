using System;
using System.Threading.Tasks;

namespace TellIt
{
    public interface IPlotTap
    {
        void Listen<T>(Action<T, SceneActor> callbackFunc) where T : class;
        void Listen<T>(Func<T, SceneActor, Task> callbackFunc) where T : class;
    }
}