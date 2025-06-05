using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(ScriptDispatcherComponent))]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BBParser))]
    public static class ScriptHelper
    {
        public const float FrameLength = 0.0166666f;
        public const int FrameTick = 166666;
        
        public static void Reload()
        {
            CodeLoader.Instance.LoadHotfix();
            EventSystem.Instance.Load();
            Log.Debug("hot reload success");
        }
        
        public static void ScripMatchError(string text)
        {
            Log.Error($"{text}匹配失败！请检查格式");
        }
        
        public static bool Trigger(this BehaviorInfo self)
        {
            BBParser parser = self.GetParent<BehaviorMachine>().GetParent<Unit>().GetComponent<BBParser>();

            //不存在函数
            int index = parser.GetFunctionPointer(self.behaviorName, "Trigger");
            if (index < 0)
            {
                return false;
            }

            for (int i = index + 1; i < parser.OpDict.Count; i++)
            {
                string opLine = parser.OpDict[i];
                if (opLine.Equals("return;"))
                {
                    return true;
                }
                Match match = Regex.Match(opLine, @"^\w+");
                if (!match.Success)
                {
                    ScripMatchError(opLine);
                    return false;
                }
                
                //执行TriggerHandler
                BBTriggerHandler handler = ScriptDispatcherComponent.Instance.GetTrigger(match.Value);
                BBScriptData data = BBScriptData.Create(opLine, 0);
                bool ret = handler.Check(parser, data);
                if (ret is false)
                {
                    return false;
                }
            }
            
            return true;
        }

        public static bool TargetComboTrigger(this BehaviorInfo self)
        {
            BBParser parser = self.GetParent<BehaviorMachine>().GetParent<Unit>().GetComponent<BBParser>();
            
            //不存在函数
            int index = parser.GetFunctionPointer(self.behaviorName, "TargetComboTrigger");
            if (index < 0)
            {
                return false;
            }
            
            for (int i = index + 1; i < parser.OpDict.Count; i++)
            {
                string opLine = parser.OpDict[i];
                if (opLine.Equals("return;"))
                {
                    return true;
                }
                Match match = Regex.Match(opLine, @"^\w+");
                if (!match.Success)
                {
                    ScripMatchError(opLine);
                    return false;
                }
                
                //执行TriggerHandler
                BBTriggerHandler handler = ScriptDispatcherComponent.Instance.GetTrigger(match.Value);
                BBScriptData data = BBScriptData.Create(opLine, 0);
                bool ret = handler.Check(parser, data);
                if (ret is false)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
