using System.Drawing;

namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class EmojiComponent: Entity, IAwake, IDestroy
    {
        public Icon icon;
    }
}