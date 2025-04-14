using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_CreateBullet_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CreateBullet";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CreateBall: ;");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //1. 
            BulletManager.Instance.AddChild<Unit, int>(1001);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}