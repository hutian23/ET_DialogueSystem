using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(ZakoChaseComponent))]
    public class Function_EnableZakoChase_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableZakoChase";
        }

        //EnableZakoChase: Distance, Velocity;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableZakoChase: (?<Distance>.*?), (?<Velocity>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["Distance"].Value, out long distance) ||
                !long.TryParse(match.Groups["Velocity"].Value, out long velocity))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            parser.RemoveComponent<ZakoChaseComponent>();
            ZakoChaseComponent zakoChaseComponent = parser.AddComponent<ZakoChaseComponent>(true);
            zakoChaseComponent.distance = distance / 10000f;
            zakoChaseComponent.velocity = velocity / 10000f;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}