using System.Reflection;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class NumericDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "Numeric";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            //数值类型
            Match match = Regex.Match(line, @"Numeric\s+(\w+)\s*(\+|\-|\*|\/)\s*(\d+);");
            if (!match.Success) DialogueHelper.ScripMatchError(line);

            string numericTypeStr = match.Groups[1].Value;
            string opType = match.Groups[2].Value;
            string valueStr = match.Groups[3].Value;

            //属性类型
            FieldInfo fieldInfo = typeof (NumericType).GetField(numericTypeStr, BindingFlags.Public | BindingFlags.Static);
            int numericType = (int)fieldInfo.GetValue(null);
            Unit player = TODUnitHelper.GetPlayer(unit.ClientScene());
            NumericComponent nu = player.GetComponent<NumericComponent>();

            // 操作码和值
            int value = int.Parse(valueStr);
            switch (opType)
            {
                case "+":
                    nu[numericType] += value;
                    break;
                case "-":
                    nu[numericType] -= value;
                    break;
                case "/":
                    nu[numericType] /= value;
                    break;
                case "*":
                    nu[numericType] *= value;
                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}