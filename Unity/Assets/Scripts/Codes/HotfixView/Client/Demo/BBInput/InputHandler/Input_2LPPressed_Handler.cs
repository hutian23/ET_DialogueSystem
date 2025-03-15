namespace ET.Client
{
    [FriendOf(typeof(InputWait))]
    public class Input_2LPPressed_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "2LPPressed";
        }

        public override string GetBufferType()
        {
            return "2LPPressed";
        }

        public override long Handle(InputWait self)
        {
            bool direction = self.IsPressing(BBOperaType.DOWN) || self.IsPressing(BBOperaType.DOWNLEFT) || self.IsPressing(BBOperaType.DOWNRIGHT);
            return direction && self.WasPressedThisFrame(BBOperaType.X)? self.GetBuffFrame(10): -1;
        }
    }
}