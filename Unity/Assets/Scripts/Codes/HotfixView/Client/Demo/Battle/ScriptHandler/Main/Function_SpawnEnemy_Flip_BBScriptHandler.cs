using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_SpawnEnemy_Flip_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SpawnEnemy_Flip";
        }

        //SpawnEnemy_Flip: Right;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SpawnEnemy_Flip: (?<Flip>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            long instanceId = parser.GetParam<long>("SpawnEnemy_InstanceId");
            Unit enemy = Root.Instance.Get(instanceId) as Unit;
            b2Body body = b2WorldManager.Instance.GetBody(enemy.InstanceId);
            
            FlipState flip = match.Groups["Flip"].Value.Equals("Left")? FlipState.Left : FlipState.Right;
            body.SetFlip(flip);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}