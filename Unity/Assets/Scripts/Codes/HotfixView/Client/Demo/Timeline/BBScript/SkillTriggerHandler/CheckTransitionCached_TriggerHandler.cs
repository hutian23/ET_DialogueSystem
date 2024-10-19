using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckTransitionCached_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "TransitionCached";
        }

        /// <summary>
        /// note: Transition 和 TransitionCached
        /// 行为没有发生切换时，transitionFlag仍然缓存在SkillBuffer中，如果行为发生切换了，则transitionFlag缓存在parser中
        /// 在Trigger函数中应使用Transition，检测Buffer; 在Main函数中使用Cached，检测Parser
        /// </summary>
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"TransitionCached: '(?<transition>\w+)'");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }

            string transitionFlag = $"Transition_{match.Groups["transition"].Value}";
            if (!parser.ContainParam(transitionFlag))
            {
                return false;
            }

            return parser.GetParam<bool>($"Transition_{match.Groups["transition"].Value}");
        }
    }
}