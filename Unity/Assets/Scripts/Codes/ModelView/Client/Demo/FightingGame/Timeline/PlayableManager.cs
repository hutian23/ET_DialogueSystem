using Timeline;

namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class PlayableManager: Entity, IAwake, IDestroy
    {
    }
}