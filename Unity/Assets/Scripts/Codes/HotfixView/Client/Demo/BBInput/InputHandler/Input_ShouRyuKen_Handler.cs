namespace ET.Client
{
    public class Input_ShouRyuKen_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "ShouRyuKen";
        }

        public override string GetBufferType()
        {
            return "ShouRyuKen";
        }

        public override long Handle(InputWait self)
        {
            bool direction = self.IsKeyCached(BBOperaType.UP) || self.IsKeyCached(BBOperaType.UPLEFT) || self.IsKeyCached(BBOperaType.UPRIGHT);
            return direction && self.WasPressedThisFrame(BBOperaType.X)? self.GetBuffFrame(15): -1;
        }
    }
}