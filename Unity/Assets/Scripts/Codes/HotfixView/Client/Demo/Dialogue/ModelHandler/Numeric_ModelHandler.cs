using System.Reflection;
using System.Text.RegularExpressions;
using ET.Client.V_Model;

namespace ET.Client
{
    public class Numeric_ModelHandler: ModelHandler
    {
        public override string GetModelType()
        {
            return "Numeric";
        }
        
        //<model type=Numeric name=HP/>
        public override string GetReplaceStr(Unit unit, string model)
        {
            string pattern = "name=([A-Za-z]+)";
            Regex regex = new(pattern);
            string numericName = regex.Match(model).Groups[1].Value;
            if (string.IsNullOrEmpty(numericName)) return "";
            
            FieldInfo fieldInfo = typeof (NumericType).GetField(numericName, BindingFlags.Public | BindingFlags.Static);
            int numericType = (int)fieldInfo.GetValue(null);

            Unit player = TODUnitHelper.GetPlayer(ClientSceneManagerComponent.Instance.ClientScene());
            NumericComponent nu = player.GetComponent<NumericComponent>();
            return nu[numericType].ToString();
        }
    }
}