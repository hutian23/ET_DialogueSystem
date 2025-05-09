using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(GlinBulletCaster))]
    public class RootInit_CastGlinBullet_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CastGlinBullet";
        }

        //CastGlinBullet: interval, targetPosX, targetPosY, offset, bulletCount;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CastGlinBullet: (?<interval>.*?), (?<targetPosX>.*?), (?<targetPosY>.*?), (?<offset>.*?), (?<bulletCount>.*?), (?<ShakeLength_X>.*?), (?<ShakeLength_Y>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["interval"].Value, out int interval) ||
                !long.TryParse(match.Groups["targetPosX"].Value, out long targetPosX) ||
                !long.TryParse(match.Groups["targetPosY"].Value, out long targetPosY) ||
                !long.TryParse(match.Groups["offset"].Value, out long offset)||
                !int.TryParse(match.Groups["bulletCount"].Value, out int bulletCount) ||
                !long.TryParse(match.Groups["ShakeLength_X"].Value, out long shakeLength_X) ||
                !long.TryParse(match.Groups["ShakeLength_Y"].Value, out long shakeLength_Y) ||
                !long.TryParse(match.Groups["Frequency"].Value, out long frequency) ||
                !int.TryParse(match.Groups["ShakeFrame"].Value, out int shakeFrame))
            {
                Log.Error($"match failed");
                return Status.Failed;
            }

            parser.RemoveComponent<GlinBulletCaster>();
            GlinBulletCaster bulletCaster = parser.AddComponent<GlinBulletCaster>();

            Vector2 center = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId).GetPosition(); 
            
            bulletCaster.interval = interval;
            bulletCaster.targetPos = center + new Vector2(targetPosX, targetPosY) / 10000f;
            bulletCaster.cnt = bulletCount;
            bulletCaster.offset = offset / 10000f;
            bulletCaster.shakeLengthX = shakeLength_X / 10000f;
            bulletCaster.shakeLengthY = shakeLength_Y / 10000f;
            bulletCaster.frequency = frequency / 10000f;
            bulletCaster.shakeFrame = shakeFrame;
            
            bulletCaster.token = new ETCancellationToken();
            bulletCaster.CastCor().Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}