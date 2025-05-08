using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_Visible_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Visible";
        }

        //Visible: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Visible: (?<Active>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            GameObject go = parser.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            go.SetActive(match.Groups["Active"].Value.Equals("true"));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}