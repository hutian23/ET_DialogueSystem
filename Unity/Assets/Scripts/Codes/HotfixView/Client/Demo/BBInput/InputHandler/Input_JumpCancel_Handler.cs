namespace ET.Client
{
    [FriendOf(typeof(InputWait))]
    public class Input_JumpCancel_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "JumpCancel";
        }

        public override string GetBufferType()
        {
            return "JumpCancel";
        }

        public override long Handle(InputWait self)
        {
            if (self.IsPressing(BBOperaType.UP) || self.IsPressing(BBOperaType.UPLEFT) || self.IsPressing(BBOperaType.UPRIGHT))
            {
                // // 跳跃取消的朝向
                // if (!self.BufferDict.ContainsKey("JumpCancel_Left") || !self.BufferDict.ContainsKey("JumpCancel_Right"))
                // {
                //     self.BufferDict.TryAdd("JumpCancel_Left", -1);
                //     self.BufferDict.TryAdd("JumpCancel_Right", -1);
                // }
                //
                // self.BufferDict["JumpCancel_Left"] = self.GetBuffFrame(5);
                // self.BufferDict["JumpCancel_Right"] = self.GetBuffFrame(5);
                //
                return self.GetBuffFrame(5);
            }
            return -1;
        }
    }
}