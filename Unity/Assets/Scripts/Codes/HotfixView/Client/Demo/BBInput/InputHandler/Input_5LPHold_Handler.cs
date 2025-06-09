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

        public override long Handle(InputComponent self)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            
            //1. 按住X超过20帧，如果超过30帧认为没有按下
            long curFrame = sceneTimer.GetNow();
            long pressedFrame = self.GetPressedFrame(BBOperaType.X);
            return self.IsPressing(BBOperaType.X) && curFrame - pressedFrame >= 20 && curFrame - pressedFrame <= 30? self.GetBuffFrame(30): -1;
        }
    }
}