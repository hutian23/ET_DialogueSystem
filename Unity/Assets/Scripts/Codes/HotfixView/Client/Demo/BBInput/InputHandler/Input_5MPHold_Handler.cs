namespace ET.Client
{
    public class Input_5MPHold_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "5MPHold";
        }

        public override string GetBufferType()
        {
            return "5MPHold";
        }

        public override long Handle(InputWait self)
        {
            return self.IsPressing(BBOperaType.Y)? self.GetBuffFrame(10) : -1;
        }
    }
}