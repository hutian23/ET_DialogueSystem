using ET.Client.V_Model;

namespace ET.Client
{
    public class NumericReplaceHandler: ReplaceHandler
    {
        public override string GetReplaceType()
        {
            return "Numeric";
        }

        public override string GetReplaceStr(Unit unit, string model)
        {
            return "Hello world";
        }
    }
}