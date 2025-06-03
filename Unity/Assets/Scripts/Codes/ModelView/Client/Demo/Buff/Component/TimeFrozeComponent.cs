namespace ET.Client
{
    [ComponentOf]
    public class TimeFrozeComponent : Entity, IAwake<int, int, long>, IDestroy, IFrameUpdate
    {
        public int Hertz;      // 设置buff期间的Hertz 
        public int LastFrame;  // 持续帧数
        public int cnt;
        public long unitId;    // Destroy生命周期中 unit.instanceId为0, 无法访问物理层的b2body
    }
}