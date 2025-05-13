namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class AccelXComponent : Entity, IAwake<float, float, int>, IDestroy, IPostStep
    {
        public float startX;
        public float currentX;
        public float accelX;
        public int lastFrame;
        public int cnt;
    }
}