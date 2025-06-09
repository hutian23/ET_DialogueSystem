namespace ET.Client
{
    public class Input_8MPPressed_BBScriptHandler: InputHandler
    {
        public override string GetHandlerType()
        {
            return "8MPPressed";
        }

        public override string GetBufferType()
        {
            return "8MPPressed";
        }

        public override long Handle(InputComponent self)
        {
            bool direction = self.IsPressing(BBOperaType.UP) || self.IsPressing(BBOperaType.UPLEFT) || self.IsPressing(BBOperaType.UPRIGHT);
            return direction && self.WasPressedThisFrame(BBOperaType.Y)? self.GetBuffFrame(12): -1;
        }
    }
}