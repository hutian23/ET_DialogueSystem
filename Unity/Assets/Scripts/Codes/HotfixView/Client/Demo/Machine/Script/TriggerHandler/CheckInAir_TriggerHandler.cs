using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckInAir_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "InAir";
        }

        //inAir: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"InAir: (?<InAir>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            //2. 查询组件
            // Unit unit = parser.GetParent<Unit>();
            // AirCheckComponent airCheck = unit.GetComponent<AirCheckComponent>();
            // if (airCheck == null)
            // {
            //     Log.Error($"does not exist AirCheckComponent!");
            //     return false;
            // }
            //
            // switch (match.Groups["inAir"].Value)
            // {
            //     case "true":
            //         return airCheck.GetInAir();
            //     case "false":
            //         return !airCheck.GetInAir();
            //     default:
            //         Log.Error("does not match inAir!");
            //         return false;
            // }

            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            
            bool ret = false;
            switch (match.Groups["InAir"].Value)
            {
                case "true":
                    ret = machine.GetParam<bool>("InAir");
                    break;
                case "false":
                    ret = !machine.GetParam<bool>("InAir");
                    break;
                default:
                    ScriptHelper.ScripMatchError(data.opLine);
                    throw new Exception();
            }
            
            return ret;
        }
    }
}