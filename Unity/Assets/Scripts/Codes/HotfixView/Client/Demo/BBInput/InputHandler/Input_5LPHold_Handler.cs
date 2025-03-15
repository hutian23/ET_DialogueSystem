namespace ET.Client
{
    public class Input_5LPHold_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "5LPHold";
        }

        public override string GetBufferType()
        {
            return "5LPHold";
        }

        public override long Handle(InputWait self)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            long curFrame = sceneTimer.GetNow();
            long pressedFrame = self.GetPressedFrame(BBOperaType.X);
            
            return self.IsPressing(BBOperaType.X) && curFrame - pressedFrame> 25 && curFrame - pressedFrame < 30? self.GetBuffFrame(30): -1;
        }
    }
}