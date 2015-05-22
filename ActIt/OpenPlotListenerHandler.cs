using System.Threading.Tasks;

namespace ActIt
{
    public abstract class OpenPlotListenerHandler<TEvent>
    {
        public abstract Task Handle(TEvent theEvent, SceneActor actor);
    }
}