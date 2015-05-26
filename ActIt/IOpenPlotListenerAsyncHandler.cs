using System.Threading.Tasks;

namespace ActIt
{
    public interface IOpenPlotListenerAsyncHandler<in TEvent>
    {
        Task Handle(TEvent theEvent, SceneActor actor);
    }
}