using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(GlobalComponent))]
    [FriendOf(typeof(BBParser))]
    public class Function_SpawnEnemy_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SpawnEnemy";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SpawnEnemy: (?<EnemyName>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            // 创建GameObject
            string bundleName = $"{match.Groups["EnemyName"].Value.ToLower()}.unity3d";
            await ResourcesComponent.Instance.LoadBundleAsync(bundleName);
            GameObject prefab = ResourcesComponent.Instance.GetAsset(bundleName, match.Groups["EnemyName"].Value) as GameObject;
            GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
            
            // 创建Unit
            // 我们可以认为,在运行时创建的敌人、子弹都是Bullet, 需要在热重载时销毁这些Unit.
            // 也许运行时生成的所有Unit都可以挂载在BulletManager下?
            Unit enemy = BattleSceneManager.Instance.AddChild<Unit, int>(1001);
            enemy.AddComponent<GameObjectComponent>().GameObject = go;
            enemy.AddComponent<BBParser>();
            go.GetComponent<BBScript>().instanceId = enemy.InstanceId;

            // 初始化Enemy
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndSpawnEnemy:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;
            
            parser.RegistParam("SpawnEnemy_InstanceId", enemy.InstanceId);
            parser.RegistSubCoroutine(startIndex, endIndex, token).Coroutine();
            parser.TryRemoveParam("SpawnEnemy_InstanceId");

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}