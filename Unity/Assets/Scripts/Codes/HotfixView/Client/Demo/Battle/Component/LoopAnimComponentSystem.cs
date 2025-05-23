using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(LoopAnimComponent))]
    public static class LoopAnimComponentSystem
    {
        public class LoopAnimComponentAwakeSystem : AwakeSystem<LoopAnimComponent>
        {
            protected override void Awake(LoopAnimComponent self)
            {
                self.triggerIndex = 0;
                self.spriteQueue.Clear();
                self.token = new ETCancellationToken();
            }
        }
        
        [FriendOf(typeof(BBParser))]
        public class LoopAnimComponentFrameLateUpdateSystem : FrameLateUpdateSystem<LoopAnimComponent>
        {
            protected override void FrameLateUpdate(LoopAnimComponent self)
            {
                BBParser parser = self.GetParent<BBParser>();

                //1. Match trigger
                string loopTrigger = parser.OpDict[self.triggerIndex];
                MatchCollection matches = Regex.Matches(loopTrigger, @"\((.*?)\)");
                if (matches.Count == 0)
                {
                    Log.Error($"Loop_Handler must have at least one triggerHandler!");
                    return;
                }

                //2. Exec trigger
                for (int i = 0; i < matches.Count; i++)
                {
                    string op = matches[i].Groups[1].Value;
                    Match triggerMatch = Regex.Match(op, "(.*?):");

                    // Match Failed
                    if (!triggerMatch.Success)
                    {
                        ScriptHelper.ScripMatchError(op);
                        break;
                    }

                    // Match Success
                    BBScriptData _data = BBScriptData.Create(op, 0);
                    bool ret = ScriptDispatcherComponent.Instance.GetTrigger(triggerMatch.Groups[1].Value).Check(parser, _data);
                    if (ret) continue;

                    // Cancel LoopAnimCor
                    self.Dispose();
                    return;
                }
            }
        }

        public class LoopAnimComponentDestroySystem : DestroySystem<LoopAnimComponent>
        {
            protected override void Destroy(LoopAnimComponent self)
            {
                self.triggerIndex = 0;
                self.spriteQueue.Clear();
                self.token.Cancel();
            }
        }

        public static async ETTask<Status> LoopAnimCor(this LoopAnimComponent self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            
            // 至少一个关键帧才会进行播放
            if (self.spriteQueue.Count == 0) return Status.Success;

            while (true)
            {
                int count = self.spriteQueue.Count;
                while (count -- > 0)
                {
                    LoopSpriteDef def = self.spriteQueue.Dequeue();
                    self.spriteQueue.Enqueue(def);
                    
                    // 播放关键帧
                    timelineComponent.Evaluate(def.spriteFrame);
                    // 等待n帧
                    await bbTimer.WaitFrameAsync(self.token);
                    if (self.token.IsCancel()) return Status.Failed;
                }
            }
        }
    }
}