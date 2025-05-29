namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class HardLandCheckComponent : Entity, IAwake, IDestroy, IPostStep, IAwake<int, float>
    {
        public bool HardLand;
        public float airVel;
        public int waitFrame;
        public int cnt;
    }
}