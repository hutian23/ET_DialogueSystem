namespace ET.Client
{
    //视觉小说里很常见的一个需求,可以从任意位置SL退出。然后从中断的位置重开。
    //比如我打逆转，出示证物的时候，实在想不出来是哪个，可以用SL大法把所有证物都出示一遍(证据就是...我的律师徽章!)
    //因为这是一个以节点为单位的对话系统(节点包含条件、行为)，从对话树任意位置进入对话树都无问题。
    [ComponentOf(typeof (DialogueStorage))]
    public class VN_CurrentNode: Entity, IAwake, IDestroy
    {
        public long ID;
    }
}