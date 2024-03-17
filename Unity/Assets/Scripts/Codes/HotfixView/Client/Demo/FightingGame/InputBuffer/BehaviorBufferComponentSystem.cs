using System.Text.RegularExpressions;
using Sirenix.Utilities;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorBufferComponent))]
    [FriendOf(typeof (InputCheck))]
    [FriendOf(typeof (BehaviorInfo))]
    public static class BehaviorBufferComponentSystem
    {
        [Invoke(BBTimerInvokeType.BehaviorTimer)]
        [FriendOf(typeof (BehaviorBufferComponent))]
        [FriendOf(typeof (BehaviorInfo))]
        public class BehaviorTimer: BBTimer<BehaviorBufferComponent>
        {
            protected override void Run(BehaviorBufferComponent self)
            {
                //1. 接收输入，并缓冲指令对应的行为(eg. 2626LP --->缓冲 波动拳和真升龙拳)
                BBInputComponent bbInput = self.GetParent<DialogueComponent>().GetComponent<BBInputComponent>();
                long ops = bbInput.CheckInput();
                bbInput.GetComponent<BBWait>().Notify(ops);

                //2.每帧检测条件，符合条件缓存行为 
                BehaviorBufferComponent bufferComponent = self.GetParent<DialogueComponent>().GetComponent<BehaviorBufferComponent>();
                bufferComponent.triggerCheckDict.Values.ForEach(triggerCheck => { triggerCheck.Check(); });

                //3. 清理过期缓存，得到每帧所有可执行的行为
                BBTimerComponent bbtimer = self.GetParent<DialogueComponent>().GetComponent<BBTimerComponent>();
                int count = self.BufferQueue.Count;
                self.OrderSet.Clear();
                while (count-- > 0)
                {
                    BehaviorBuffer buffer = self.BufferQueue.Dequeue();
                    //检查是否过期，过期则回收
                    if (bbtimer.GetNow() > buffer.startFrame + buffer.LastedFrame)
                    {
                        buffer.Recycle();
                        continue;
                    }

                    //当前帧检测条件
                    BehaviorInfo info = self.behaviorDict[buffer.targetID];
                    bool ret = true;
                    foreach (string trigger in info.triggers)
                    {
                        Match match = Regex.Match(trigger, @"^\w+");
                        if (!match.Success)
                        {
                            DialogueHelper.ScripMatchError(trigger);
                            return;
                        }

                        BBTriggerHandler handler = DialogueDispatcherComponent.Instance.GetTrigger(match.Value);
                        BBParser parser = self.GetParent<DialogueComponent>().GetComponent<BBParser>();
                        BBScriptData data = BBScriptData.Create(trigger, 0, buffer.targetID);
                        bool res = handler.Check(parser, data);
                        data.Recycle();

                        if (res) continue;
                        ret = false;
                        break;
                    }

                    if (!ret) continue;
                    //缓存当前帧所有可执行的行为
                    self.OrderSet.Add(buffer.order);
                    self.BufferQueue.Enqueue(buffer);
                }
            }
        }

        public static void EnableBufferCheck(this BehaviorBufferComponent self, ETCancellationToken token)
        {
            self.GetParent<DialogueComponent>()
                    .GetComponent<BBInputComponent>()
                    .GetComponent<BBTimerComponent>()
                    .NewFrameTimer(BBTimerInvokeType.BehaviorBufferCheckTimer, self);
            self.inputCheckDict.Values.ForEach(inputCheck => { InputCheckCor(inputCheck, token).Coroutine(); });
        }

        private static async ETTask InputCheckCor(InputCheck inputCheck, ETCancellationToken token)
        {
            while (true)
            {
                await inputCheck.InputCheckCor(token);
                if (token.IsCancel()) return;

                await TimerComponent.Instance.WaitFrameAsync(token);
                if (token.IsCancel()) return;
            }
        }

        public static void ClearGC(this BehaviorBufferComponent self)
        {
            self.GCSet.Clear();
        }

        public static void ClearWhiff(this BehaviorBufferComponent self)
        {
            self.WhiffSet.Clear();
        }

        private static void Init(this BehaviorBufferComponent self)
        {
            self.OrderSet.Clear();
            self.orderDict.Clear();
            self.tagDict.Clear();
            self.GCSet.Clear();
            self.WhiffSet.Clear();
            self.BufferQueue.ForEach(buffer => { buffer.Recycle(); });
            self.GetParent<DialogueComponent>()
                    .GetComponent<BBInputComponent>()?
                    .GetComponent<BBTimerComponent>()?
                    .Remove(ref self.bufferTimer);
        }

        public static void AddBuffer(this BehaviorBufferComponent self, long order, long lastedFrame, uint targetID)
        {
            BBTimerComponent bbtimer = self.GetParent<DialogueComponent>().GetComponent<BBTimerComponent>();
            BehaviorBuffer buffer = BehaviorBuffer.Create(order, bbtimer.GetNow(), lastedFrame, targetID);
            self.BufferQueue.Enqueue(buffer);
        }

        public static uint GetTargetID(this BehaviorBufferComponent self, long order)
        {
            if (self.orderDict.TryGetValue(order, out uint targetID))
            {
                return targetID;
            }

            Log.Error($"not exist behaviorInfo, order: {order}");
            return 0;
        }

        public static uint GetTargetID(this BehaviorBufferComponent self, string tag)
        {
            if (self.tagDict.TryGetValue(tag, out uint targetID))
            {
                return targetID;
            }

            Log.Error($"not exist behaviorInfo,tag: {tag}");
            return 0;
        }

        public static long GetOrder(this BehaviorBufferComponent self, uint targetID)
        {
            if (self.behaviorDict.TryGetValue(targetID, out BehaviorInfo info))
            {
                return info.GetOrder();
            }

            return -1;
        }

        public static long GetOrder(this BehaviorBufferComponent self, string tag)
        {
            uint targetID = self.GetTargetID(tag);
            return self.GetOrder(targetID);
        }
        
        public class BehaviorBufferComponentDestroySystem: DestroySystem<BehaviorBufferComponent>
        {
            protected override void Destroy(BehaviorBufferComponent self)
            {
                self.Init();
            }
        }
    }
}