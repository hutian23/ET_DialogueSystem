using System;
using System.Text.RegularExpressions;
using ET.Client.V_Model;

namespace ET.Client
{
    public class UnitConfig_ModelHandler: ModelHandler
    {
        public override string GetModelType()
        {
            return "UnitConfig";
        }
            
        //<model type=UnitConfig name=Name (id=1001)[可选，如果存在这个属性，则从unitConfig.xlsx中查找]/>
        public override string GetReplaceStr(Unit unit, string modelName)
        {
            //1. 是否为外部的unitConfig
            string pattern = @"id=(\d+)";
            Regex regex1 = new(pattern);
            string id = regex1.Match(modelName).Groups[1].Value;

            UnitConfig config;
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    config = UnitConfigCategory.Instance.Get(int.Parse(id));
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
            config = unit.Config;
               
            //2. 判断属性类型
            string pattern2 = "name=([A-Za-z]+)";
            Regex regex2 = new(pattern2);
            string name = regex2.Match(modelName).Groups[1].Value;

            switch (name)
            {
                case "Name":
                    return config.Name;
                case "Type":
                    return Enum.ToObject(typeof (UnitType), config.Type).ToString();
                default:
                    return "";
            }
        }
    }
}