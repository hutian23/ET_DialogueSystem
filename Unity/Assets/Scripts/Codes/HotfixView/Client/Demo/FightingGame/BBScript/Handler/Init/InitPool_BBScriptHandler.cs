using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class InitPool_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "InitPool";
        }

        //InitPool name = HoldIt size = 5; 
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            Match match = Regex.Match(opCode, @"InitPool name = (?<poolName>\w+) size = (?<poolSize>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }

            var prefabName = match.Groups["poolName"].Value;
            int.TryParse(match.Groups["poolSize"].Value, out int poolSize);

            await ResourcesComponent.Instance.LoadBundleAsync($"{prefabName}.unity3d");
            GameObject prefab = ResourcesComponent.Instance.GetAsset($"{prefabName}.unity3d", prefabName) as GameObject;
            await GameObjectPoolHelper.InitPoolFormGamObjectAsync(prefab, poolSize);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}