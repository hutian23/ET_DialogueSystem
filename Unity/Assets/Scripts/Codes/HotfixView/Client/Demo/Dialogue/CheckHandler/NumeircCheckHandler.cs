using System.Reflection;

namespace ET.Client
{
    public class NumeircCheckHandler: NodeCheckHandler<NumericCheckConfig>
    {
        protected override int Run(Unit unit, NumericCheckConfig nodeCheck)
        {
            Unit player = TODUnitHelper.GetPlayer(unit.ClientScene());

            if (string.IsNullOrEmpty(nodeCheck.NumericType)) return 1;
            
            FieldInfo fieldInfo = typeof (NumericType).GetField(nodeCheck.NumericType, BindingFlags.Public | BindingFlags.Static);
            int numericType = (int)fieldInfo.GetValue(null);
            int numeric = player.GetComponent<NumericComponent>().GetAsInt(numericType);

            return nodeCheck.CheckType switch
            {
                NumericCheckerType.Equal => (numeric == nodeCheck.EqualValue)? 0 : 1,
                NumericCheckerType.InRange => (numeric >= nodeCheck.minValue && numeric <= nodeCheck.maxValue)? 0 : 1,
                NumericCheckerType.LessThan => (numeric < nodeCheck.CompareValue)? 0 : 1,
                NumericCheckerType.MoreThan => (numeric > nodeCheck.CompareValue)? 0 : 1,
                _ => 1
            };
        }
    }
}