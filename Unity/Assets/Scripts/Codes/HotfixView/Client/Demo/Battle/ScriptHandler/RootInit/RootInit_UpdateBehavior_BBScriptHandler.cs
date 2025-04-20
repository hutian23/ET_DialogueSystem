namespace ET.Client
{
    public class RootInit_UpdateBehavior_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "UpdateBehavior";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            InputWait inputWait = unit.GetComponent<InputWait>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            
            //1. 切换行为时，根据按键输入更新朝向
            if (inputWait.IsPressing(BBOperaType.LEFT) ||
                inputWait.IsPressing(BBOperaType.DOWNLEFT) ||
                inputWait.IsPressing(BBOperaType.UPLEFT))
            {
                body.SetFlip(FlipState.Left);
            }
            else if (inputWait.IsPressing(BBOperaType.RIGHT) ||
                     inputWait.IsPressing(BBOperaType.DOWNRIGHT) ||
                     inputWait.IsPressing(BBOperaType.UPRIGHT))
            {
                body.SetFlip(FlipState.Right);
            }
            
            //2. 
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}