namespace ET.Client
{
    public class Input_5MPPressing_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "5MPPressing";
        }

        public override string GetBufferType()
        {
            return "5MPPressing";
        }

        public override long Handle(InputComponent self)
        {
            return self.IsPressing(BBOperaType.Y)? self.GetBuffFrame(1) : -1;
        }
    }
}