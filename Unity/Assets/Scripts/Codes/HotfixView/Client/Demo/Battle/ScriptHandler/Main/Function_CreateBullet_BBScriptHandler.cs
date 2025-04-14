using System.Text.RegularExpressions;
using UnityEngine;

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
            Match match = Regex.Match(data.opLine, @"CreateBullet: (?<BulletName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //1. 创建Bullet Unit
            Unit unit = BulletManager.Instance.AddChild<Unit, int>(1001);
            
            GameObject bullet = GameObjectPoolHelper.GetObjectFromPool(match.Groups["BulletName"].Value);
            unit.AddComponent<GameObjectComponent>().GameObject = bullet;
            
                    
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}