using System.Text.RegularExpressions;

namespace ET.Client
{
    public class ConstantReplaceHandler: ReplaceHandler
    {
        public override string GetReplaceType()
        {
            return "Constant";
        }

        //<Constant name=test/>;
        public override string GetReplaceStr(Unit unit, string model)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            Match match = Regex.Match(model, @"<Constant name=(\w+)");
            if (!match.Success) DialogueHelper.ScripMatchError(model);

            var constantsName = match.Groups[1].Value;
            return dialogueComponent.GetConstant<object>(constantsName).ToString();
        }
    }
}