using System.Text.RegularExpressions;
using Sirenix.Utilities;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorBufferComponent))]
    [FriendOf(typeof (InputCheck))]
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
                self.OrderList.Clear();
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
                    self.OrderList.Add(buffer.order);
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

        public static bool ContainOrder(this BehaviorBufferComponent self, uint skillType, uint order)
        {
            ulong result = 0;
            result |= order;
            result |= (ulong)skillType << 16;
            return self.ContainOrder((long)result);
        }

        private static bool ContainOrder(this BehaviorBufferComponent self, long skillOrder)
        {
            return self.OrderList.Contains(skillOrder);
        }

        private static void Init(this BehaviorBufferComponent self)
        {
            self.OrderList.Clear();
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
            if (self.targetIDDict.TryGetValue(order, out uint targetID))
            {
                return targetID;
            }
            Log.Error($"not found behaviorInfo ,order: {order}");
            return 0;
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