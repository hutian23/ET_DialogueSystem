using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorBufferComponent))]
    [FriendOf(typeof (TriggerCheck))]
    [FriendOf(typeof (BehaviorInfo))]
    public static class TriggerCheckSystem
    {
        public static void Check(this TriggerCheck self)
        {
            if (self.GetParent<BehaviorBufferComponent>().behaviorDict.TryGetValue(self.targetID, out BehaviorInfo info))
            {
                Log.Error($"not found behaviorInfo: {self.targetID}");
                return;
            }

            BBParser parser = self.GetParent<BehaviorBufferComponent>().GetParent<DialogueComponent>().GetComponent<BBParser>();
            foreach (string trigger in info.triggers)
            {
                Match match = Regex.Match(trigger, @"^\w+");
                if (!match.Success)
                {
                    DialogueHelper.ScripMatchError(trigger);
                    return;
                }

                BBTriggerHandler handler = DialogueDispatcherComponent.Instance.GetTrigger(match.Value);
                BBScriptData data = BBScriptData.Create(trigger, 0, self.targetID);
                bool res = handler.Check(parser, data);
                data.Recycle();

                //判定失败
                if (!res)
                {
                    return;
                }
            }

            BehaviorBufferComponent bufferComponent = self.GetParent<BehaviorBufferComponent>();
            bufferComponent.AddBuffer(info.GetOrder(), self.lastedFrame, self.targetID);
        }
    }
}