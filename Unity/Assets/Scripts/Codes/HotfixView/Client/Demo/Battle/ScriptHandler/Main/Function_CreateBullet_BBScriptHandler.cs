using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class Function_CreateBullet_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CreateBullet";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CreateBullet: (?<BulletName>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //1. 创建Bullet Unit
            Unit caster = parser.GetParent<Unit>();
            Unit unit = BulletManager.Instance.AddChild<Unit, int>(1001);
            
            //2. 添加组件
            GameObject bullet = GameObjectPoolHelper.GetObjectFromPool(match.Groups["BulletName"].Value);
            unit.AddComponent<GameObjectComponent>().GameObject = bullet;
            unit.AddComponent<BBParser>();

            //3. 对Bullet进行初始化
            //3-1 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndCreateBullet:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;
            
            //3-2 更新Bullet朝向
            b2Body bodyA = caster.GetComponent<b2Body>();
            b2Body bodyB = unit.GetComponent<b2Body>();
            bodyB.SetFlip((FlipState)bodyA.GetFlip());
            
            //3-3 执行代码块
            parser.RegistParam("CreateBullet_UnitId", unit.InstanceId);
            parser.RegistSubCoroutine(startIndex, endIndex, token).Coroutine();
            parser.TryRemoveParam("CreateBullet_UnitId");
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}