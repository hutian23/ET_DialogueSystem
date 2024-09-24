namespace ET.Client
{
    public class BehaviorBufferCheckType_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BehaviorBufferCheckType";
        }
        
        //BehaviorBufferCheckType: Input; 输入检测协程，满足指令条件，会把行为缓冲到BehaviorBufferComponent中
        //BehaviorBufferCheckType: Trigger; 每帧计时器,当前帧符合条件(SkillTrigger)，把行为缓冲到buffer中
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}