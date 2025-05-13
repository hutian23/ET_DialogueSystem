using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_GlinPos_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GlinPos";
        }

        //GlinPosX: minX, maxX, distanceX, PosY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"GlinPos: (?<minX>.*?), (?<maxX>.*?), (?<distanceX>.*?), (?<posY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["minX"].Value, out long minX) ||
                !long.TryParse(match.Groups["maxX"].Value, out long maxX) ||
                !long.TryParse(match.Groups["distanceX"].Value, out long distanceX) ||
                !long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"matched Failed");
                return Status.Failed;
            }
            
            Unit player = BBUnitHelper.GetPlayer(parser.ClientScene());
            Unit unit = parser.GetParent<Unit>();
            b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(unit.InstanceId);
            
            //1. 生成在玩家附近
            float _minX = minX / 10000f, _maxX = maxX / 10000f;
            float x1 = bodyA.GetPosition().X - distanceX / 10000f;
            float x2 = bodyA.GetPosition().X + distanceX / 10000f;

            //2. 随机选择生成在玩家的左 or 右
            int ran = 2 * new Random().Next(0, 2) - 1;
            float x = x1, y = posY / 10000f;
            FlipState flipState = FlipState.Left;
            
            //3. 限制生成位置在场地内
            if (ran == -1)
            {
                x = x1 >= _minX && x1 <= _maxX ? x1 : x2;
                flipState = x1 >= _minX && x1 <= _maxX ? FlipState.Right : FlipState.Left;
            }
            else
            {
                x = x2 >= _minX && x2 <= _maxX ? x2 : x1;
                flipState = x2 >= _minX && x2 <= _maxX ? FlipState.Left : FlipState.Right;
            }
            bodyB.SetPosition(new Vector2(x, y));
            bodyB.SetFlip(flipState);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}