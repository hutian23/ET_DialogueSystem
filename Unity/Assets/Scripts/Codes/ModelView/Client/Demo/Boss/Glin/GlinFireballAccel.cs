namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class GlinFireballAccel : Entity, IAwake<float, float>, IDestroy, IFrameUpdate
    {
        public float startY;
        public float accelY;
    }
}