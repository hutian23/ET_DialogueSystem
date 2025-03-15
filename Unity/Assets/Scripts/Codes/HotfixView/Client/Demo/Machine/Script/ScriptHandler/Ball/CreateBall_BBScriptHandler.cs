using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(EnemyManager))]
    [FriendOf(typeof(BallManager))]
    public class CreateBall_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CreateBall";
        }

        //CreateBall: SlashRing
        //EndCreateBall:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CreateBall: (?<ballType>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //1. 生成ball unit
            UnitComponent unitComponent = parser.ClientScene().CurrentScene().GetComponent<UnitComponent>();
            Unit ball = unitComponent.AddChild<Unit, int>(1001);

            //2. 对象池获得GameObject
            string ballType = match.Groups["ballType"].Value;
            GameObject ballGo = GameObjectPoolHelper.GetObjectFromPool(ballType);
            ballGo.transform.SetParent(GlobalComponent.Instance.Unit);
            ball.AddComponent<GameObjectComponent>().GameObject = ballGo;

            //3. 添加组件
            // TimelineComponent timelineComponent = ball.AddComponent<TimelineComponent>();
            // timelineComponent.AddComponent<BBTimerComponent>().IsUnitTimer();
            // timelineComponent.AddComponent<b2Unit, long>(ball.InstanceId);
            // timelineComponent.AddComponent<ObjectWait>();
            // timelineComponent.AddComponent<BehaviorBuffer>();
            // timelineComponent.AddComponent<BBParser, int>(ProcessType.TimelineProcess);
            // BallManager.Instance.InstanceIds.Enqueue(ball.InstanceId);
            //
            // //4. 跳过代码块
            // int index = parser.Coroutine_Pointers[data.CoroutineID];
            // int endIndex = index, startIndex = index;
            // while (++index < parser.OpDict.Count)
            // {
            //     string opLine = parser.OpDict[index];
            //     if (opLine.Equals("EndCreateBall:"))
            //     {
            //         endIndex = index;
            //         break;
            //     }
            // }
            // parser.Coroutine_Pointers[data.CoroutineID] = endIndex;
            //
            // //5. 注册变量
            // parser.TryRemoveParam("BallId");
            // parser.RegistParam("BallId", ball.InstanceId);
            // Status ret = await parser.RegistSubCoroutine(startIndex, endIndex, token);
            // parser.TryRemoveParam("BallId");
            //
            // //6. 启动ball行为机
            // timelineComponent.RegistParam("SpawnerId", parser.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            // timelineComponent.GetComponent<BBParser>().Start();

            // return token.IsCancel() || ret != Status.Success ? Status.Failed : Status.Success;
            return Status.Success;
        }
    }
}