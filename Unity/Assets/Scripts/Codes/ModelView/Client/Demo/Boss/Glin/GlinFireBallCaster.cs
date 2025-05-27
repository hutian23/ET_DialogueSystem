namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class GlinFireBallCaster : Entity, IAwake, IDestroy
    {
        public int lastFrame; // 组件的持续帧数
        public int waitFrame;
        public float startV; // 弹道初始速度
        public float accelX;
        public float accelY;
        public ETCancellationToken token;
    }
}