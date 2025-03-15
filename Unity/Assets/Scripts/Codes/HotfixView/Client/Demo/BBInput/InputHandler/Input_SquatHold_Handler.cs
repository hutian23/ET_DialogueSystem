namespace ET.Client
{
    public class Input_SquatHold_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "SquatHold";
        }

        public override string GetBufferType()
        {
            return "SquatHold";
        }

        public override long Handle(InputWait self)
        {
            return self.IsPressing(BBOperaType.DOWN) || self.IsPressing(BBOperaType.DOWNLEFT) || self.IsPressing(BBOperaType.DOWNRIGHT) ? self.GetBuffFrame(5) : -1;
        }
    }
}