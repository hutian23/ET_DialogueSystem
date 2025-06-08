using System.Numerics;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class JustEvadeCheck : Entity, IAwake, IDestroy, IPostStep
    {
        // 记录 unit.instanceId
        public long _instanceId;
        public int functionIndex;
        public int lastFrame; // 精准闪避窗口持续帧数
        public Vector2 boxSize; // 判定框数据
        public Vector2 boxOffset;
        public ETCancellationToken token;
    }
}