using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class LoopAnimComponent : Entity, IAwake, IDestroy, IFrameLateUpdate
    {
        public int triggerIndex;
        public Queue<LoopSpriteDef> spriteQueue = new();
        public ETCancellationToken token = new();
    }

    public struct LoopSpriteDef
    {
        public int spriteFrame;
        public int waitFrame;
    }
}