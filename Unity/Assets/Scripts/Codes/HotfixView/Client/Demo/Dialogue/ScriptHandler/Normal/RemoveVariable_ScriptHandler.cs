using System.Linq;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(DialogueComponent))]
    public class RemoveVariable_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveVariable";
        }

        //RemoveVariable name = Random;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"RemoveVariable name = (?<name>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            string variableName = match.Groups["name"].Value;
            
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            SharedVariable variable = dialogueComponent.Variables.FirstOrDefault(v => v.name == variableName);
            if(variable == null) Log.Error($"not found sharedVariable: {variableName}");
            dialogueComponent.Variables.Remove(variable);


            await ETTask.CompletedTask;
        }
    }
}