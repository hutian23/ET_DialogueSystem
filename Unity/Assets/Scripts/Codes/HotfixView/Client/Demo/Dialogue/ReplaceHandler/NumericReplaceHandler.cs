using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class NumericReplaceHandler: ReplaceHandler
    {
        public override string GetReplaceType()
        {
            return "Numeric";
        }

        // <Nuemric type=Hp/> ---> Hp
        public override string GetReplaceStr(Unit unit, string model)
        {
            Match match = Regex.Match(model, @"<Numeric type=(\w+)/>");
            if (match.Success)
            {
                FieldInfo fieldInfo = typeof (NumericType).GetField(match.Groups[1].Value);
                if (fieldInfo == null) return string.Empty;
                int nuemricType = (int)fieldInfo.GetValue(null);
                NumericComponent nu = unit.GetComponent<NumericComponent>();
                return nu.GetAsInt(nuemricType).ToString();
            }

            return String.Empty;
        }
    }
}