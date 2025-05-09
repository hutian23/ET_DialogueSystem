namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class AccelYComponent : Entity, IAwake, IDestroy, IPostStep
    {
        public float startY;
        public int lastFrame;
        public int cnt;
        public float accelY;
    }
}