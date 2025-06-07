using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class Function_VFX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VFX";
        }

        // CreateEffect: Dust_1
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"VFX: (?<EffectName>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //1. 创建effect unit
            Unit effect = BattleSceneManager.Instance.AddChild<Unit, int>(1001);
            
            //2. 添加组件
            GameObject go = GameObjectPoolHelper.GetObjectFromPool(match.Groups["EffectName"].Value);
            effect.AddComponent<GameObjectComponent>().GameObject = go;
            effect.AddComponent<BBParser>();
            
            //3. 初始化effect
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndVFX:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;

            parser.RegistParam("VFX_UnitId", effect.InstanceId);
            parser.RegistSubCoroutine(startIndex, endIndex, token).Coroutine();
            parser.TryRemoveParam("VFX_UnitId");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}