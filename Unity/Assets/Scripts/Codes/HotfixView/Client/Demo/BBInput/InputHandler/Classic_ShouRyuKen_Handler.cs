namespace ET.Client
{
    [FriendOf(typeof(InputComponent))]
    public class Classic_ShouRyuKen_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "Classic_ShouRyuKen";
        }

        public override string GetBufferType()
        {
            return "ShouRyuKen";
        }

        //TODO 跳过吧，以后再做
        // 6 2 6 x 每个阶段犹豫期为5, 所以20帧前的指令无效
        public override long Handle(InputComponent self)
        {
            
            return -1;
        }
    }
}