namespace ET.Client
{
    public class Input_5MPPressed_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "5MPPressed";
        }

        public override string GetBufferType()
        {
            return "5MPPressed";
        }

        public override long Handle(InputWait self)
        {
            return self.WasPressedThisFrame(BBOperaType.Y) ? self.GetBuffFrame(10) : -1;
        }
    }
}