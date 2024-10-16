﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckFlag_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "CheckFlag";
        }

        //CheckFlag: 'RunToIdle';
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"CheckFlag: '(?<Flag>\w+)'");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }

            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            return buffer.ContainFlag(match.Groups["Flag"].Value);
        }
    }
}