namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class HitStop: Entity, IAwake<int>, IDestroy, IUpdate
    {
        public float timeScale = 1; //打击停顿默认是以60帧一秒进行停顿的。 sf6训练场可以更改游戏速度设置
        public long frameCounter = 0;
        public float deltaTimeReminder = 0;
        public long waitFrame = 0;
        public float preTimeScale = 0;
    }
}