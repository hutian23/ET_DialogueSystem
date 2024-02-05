namespace ET.Client
{
    //实现类似逆转中的 打字时角色张口说话的效果
    //1. 快进打字，回取消说话的动画 
    //2. 打字携程中停顿时间较长，也可以talk-->idle ,再次打字时idle->talk
    [ChildOf(typeof(CharacterManager))]
    public class Talker: Entity, IAwake, IDestroy, IUpdate
    {
        public string talker;
        public string idle_clip;
        public string talk_clip;
    }
}