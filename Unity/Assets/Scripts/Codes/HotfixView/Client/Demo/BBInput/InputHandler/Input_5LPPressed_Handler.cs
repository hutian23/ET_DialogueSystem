namespace ET.Client
{
    public class Input_5LPPressed_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "5LPPressed";
        }

        public override string GetBufferType()
        {
            return "5LPPressed";
        }

        public override long Handle(InputWait self)
        {
            return self.WasPressedThisFrame(BBOperaType.X) ? self.GetBuffFrame(10) : -1;
        }
    }
}