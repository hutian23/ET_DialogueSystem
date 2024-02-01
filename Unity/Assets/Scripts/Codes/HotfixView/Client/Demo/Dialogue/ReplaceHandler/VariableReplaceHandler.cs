using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VariableReplaceHandler: ReplaceHandler
    {
        public override string GetReplaceType()
        {
            return "Variable";
        }

        //<Variable name=Variable/>;
        public override string GetReplaceStr(Unit unit, string model)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            Match match = Regex.Match(model, @"<Variable name=(\w+)");
            if (!match.Success) DialogueHelper.ScripMatchError(model);

            var variableName = match.Groups[1].Value;
            return dialogueComponent.GetVariable<object>(variableName).ToString();
        }
    }
}