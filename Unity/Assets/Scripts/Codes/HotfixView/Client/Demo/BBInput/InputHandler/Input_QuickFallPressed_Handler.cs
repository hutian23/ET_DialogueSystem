namespace ET.Client
{
    public class Input_QuickFallPressed_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "QuickFallPressed";
        }

        public override string GetBufferType()
        {
            return "QuickFallPressed";
        }

        public override long Handle(InputWait self)
        {
            return self.IsPressing(BBOperaType.DOWN) && self.WasPressedThisFrame(BBOperaType.A)? self.GetBuffFrame(5) : -1;
        }
    }
}