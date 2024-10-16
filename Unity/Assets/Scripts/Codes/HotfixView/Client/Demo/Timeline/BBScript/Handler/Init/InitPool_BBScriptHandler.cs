﻿using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class InitPool_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "InitPool";
        }

        //InitPool: 'HoldIt', 5;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"InitPool: '(?<poolName>\w+)', '(?<poolSize>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
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