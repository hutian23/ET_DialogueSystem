namespace ET.Client
{
    [ComponentOf(typeof (ChainComponent))]
    public class PunchBuffer: Entity, IAwake, IDestroy
    {
        public long bufferTimer; // 移除buff

        public int moveX;
    }
}