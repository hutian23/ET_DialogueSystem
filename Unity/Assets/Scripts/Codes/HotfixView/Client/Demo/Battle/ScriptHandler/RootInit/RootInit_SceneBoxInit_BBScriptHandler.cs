namespace ET.Client
{
    public class RootInit_SceneBoxInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SceneBoxInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            unit.AddComponent<SceneBoxHandler>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}