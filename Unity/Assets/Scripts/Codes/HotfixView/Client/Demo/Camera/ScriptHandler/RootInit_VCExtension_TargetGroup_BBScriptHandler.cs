namespace ET.Client
{
    public class RootInit_VCExtension_TargetGroup_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VCExtension_TargetGroup";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();

            //1. 初始化
            parser.TryRemoveParam("VC_TargetGroup");
            
            //2. 注册变量
            ListComponent<CameraTarget> targetGroup = ListComponent<CameraTarget>.Create();
            parser.RegistParam("VC_TargetGroup", targetGroup);
            
            token.Add(() =>
            {
                targetGroup.Dispose();
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}