namespace ET.Client
{
    public class Input_DashPressed_Handler: InputHandler
    {
        public override string GetHandlerType()
        {
            return "DashPressed";
        }

        public override string GetBufferType()
        {
            return "DashPressed";
        }

        public override long Handle(InputWait self)
        {
            return self.WasPressedThisFrame(BBOperaType.B)? self.GetBuffFrame(10) : -1;
        }
    }
}