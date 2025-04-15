namespace ET.Client
{
    public class Input_5HPPressed_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "5HPPressed";
        }

        public override string GetBufferType()
        {
            return "5HPPressed";
        }

        public override long Handle(InputWait self)
        {
            return self.WasPressedThisFrame(BBOperaType.RB)? self.GetBuffFrame(10) : -1;
        }
    }
}