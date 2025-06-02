namespace ET.Client
{
    [ComponentOf(typeof (BBParser))]
    public class CircleWaveComponent: Entity, IAwake<float, float, int>, IDestroy, IFrameUpdate
    {
        public float waveWidth;
        public float waveSpeed;
        public int totalTick;
        public int currentTick;
    }
}