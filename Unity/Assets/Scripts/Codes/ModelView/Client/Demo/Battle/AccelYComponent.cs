namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class AccelYComponent : Entity, IAwake<float, float, int>, IDestroy, IPostStep
    {
        public float startY;
        public int lastFrame;
        public int cnt;
        public float accelY;
    }
}