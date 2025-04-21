namespace ET.Client
{
    [ComponentOf]
    public class TimeFrozeComponent : Entity, IAwake<int, int, long>, IDestroy
    {
        public int Hertz;      // 设置buff期间的Hertz 
        public int LastFrame;  // 持续帧数
        public long abilityId; // HertzAbility.InstanceId
        public ETCancellationToken token;
    }
}