using System.Threading.Tasks;

namespace TellIt
{
    internal delegate Task Listener(object theEvent, SceneActor actor);
}