namespace ET.Client
{
    public class Input_5HPPressing_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "5HPPressing";
        }

        public override string GetBufferType()
        {
            return "5HPPressing";
        }

        public override long Handle(InputWait self)
        {
            return self.IsPressing(BBOperaType.RB)? self.GetBuffFrame(3) : -1;
        }
    }
}