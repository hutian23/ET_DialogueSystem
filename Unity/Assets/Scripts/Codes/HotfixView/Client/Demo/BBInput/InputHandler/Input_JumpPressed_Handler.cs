namespace ET.Client
{
    public class Input_JumpPressed_Handler: InputHandler
    {
        public override string GetHandlerType()
        {
            return "JumpPressed";
        }

        public override string GetBufferType()
        {
            return "JumpPressed";
        }

        public override long Handle(InputComponent self)
        {
            return self.WasPressedThisFrame(BBOperaType.A)? self.GetBuffFrame(8) : -1;
        }
    }
}