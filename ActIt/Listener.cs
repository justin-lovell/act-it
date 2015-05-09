using System.Threading.Tasks;

namespace ActIt
{
    internal delegate Task Listener(object theEvent, SceneActor actor);
}