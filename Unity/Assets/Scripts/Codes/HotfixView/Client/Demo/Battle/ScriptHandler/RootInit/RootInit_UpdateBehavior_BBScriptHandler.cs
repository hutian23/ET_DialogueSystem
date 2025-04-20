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
            // TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            // B2Unit b2Unit = unit.GetComponent<B2Unit>();
            
            //1. 销毁当前行为协程执行中创建的共享变量
            parser.Cancel();
            // foreach (var kv in machine.tmpParamDict)
            // {
            //     parser.RegistParam(kv.Key, kv.Value.value);
            // }
            // machine.tmpParamDict.Clear();
            //2. 清空行为协程中生成的组件
            // timelineComponent.Init();
            //3. 清空缓存的碰撞信息
            // b2Unit.Init();
            
            //1. 
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
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}