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
            Unit bullet = BulletManager.Instance.AddChild<Unit, int>(1001);
            
            //2. 添加组件
            GameObject go = GameObjectPoolHelper.GetObjectFromPool(match.Groups["BulletName"].Value);
            bullet.AddComponent<GameObjectComponent>().GameObject = go;
            bullet.AddComponent<BBParser>();

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
            b2Body bodyA = b2WorldManager.Instance.GetBody(caster.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(bullet.InstanceId);
            bodyB.SetFlip(bodyA.GetFlip());
            
            //3-3 记录Bullet由谁释放，默认情况下Bullet不会和施放者碰撞
            bodyB.AddComponent<BulletCaster, long, int>(caster.InstanceId, FilterType.BulletHitFilter, true);
            
            //3-4 执行代码块
            parser.RegistParam("CreateBullet_UnitId", bullet.InstanceId);
            parser.RegistSubCoroutine(startIndex, endIndex, token).Coroutine();
            parser.TryRemoveParam("CreateBullet_UnitId");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}