namespace ET.Client
{
    public class Callback_BeforeReload_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BeforeReload";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            
            //1. 切换行为时，根据按键输入更新朝向
            InputComponent inputComponent = unit.GetComponent<InputComponent>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            if (inputComponent.IsPressing(BBOperaType.LEFT) ||
                inputComponent.IsPressing(BBOperaType.DOWNLEFT) ||
                inputComponent.IsPressing(BBOperaType.UPLEFT))
            {
                body.SetFlip(FlipState.Left);
            }
            else if (inputComponent.IsPressing(BBOperaType.RIGHT) ||
                     inputComponent.IsPressing(BBOperaType.DOWNRIGHT) ||
                     inputComponent.IsPressing(BBOperaType.UPRIGHT))
            {
                body.SetFlip(FlipState.Right);
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}