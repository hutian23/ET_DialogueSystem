using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VariableReplaceHandler: ReplaceHandler
    {
        public override string GetReplaceType()
        {
            return "Variable";
        }

        //<Variable name=test/>;
        public override string GetReplaceStr(Unit unit, string model)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            Match match = Regex.Match(model, @"<Variable name=(?<name>\w+)");
            if (!match.Success) DialogueHelper.ScripMatchError(model);

            var variableName = match.Groups["name"].Value;
            return dialogueComponent.GetShareVariable(variableName).value.ToString();
        }
    }
}