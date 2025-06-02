using System.Collections.Generic;
using System.Numerics;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(JustEvadeComponent))]
    public static class JustEvadeComponentSystem
    {
        public class JustEvadeComponentDestroySystem : DestroySystem<JustEvadeComponent>
        {
            protected override void Destroy(JustEvadeComponent self)
            {
                self.token.Cancel();
                self.boxOffset = Vector2.Zero;
                self.boxSize = Vector2.Zero;
                self.startFrame = 0;
                self.lastFrame = 0;
                self.functionIndex = 0;
                
                b2Body b2Body = b2WorldManager.Instance.GetBody(self.GetParent<BBParser>().GetParent<Unit>().InstanceId);
                b2Body.DestroyBox("JustEvadeCheckBox");
            }
        }

        [FriendOf(typeof(b2Body))]
        [FriendOf(typeof(BBParser))]
        public class JustEvadePostStepSystem : PostStepSystem<JustEvadeComponent>
        {
            protected override void PosStepUpdate(JustEvadeComponent self)
            {
                BBParser parser = self.GetParent<BBParser>();
                Unit unit = parser.GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
                b2Box b2Box = b2Body.GetBox("JustEvadeCheckBox");

                Queue<CollisionBuffer> buffQueue = b2Body.triggerStayBuffers;
                int count = buffQueue.Count;
                while (count-- > 0)
                {
                    CollisionBuffer buffer = buffQueue.Dequeue();
                    buffQueue.Enqueue(buffer);

                    b2Box boxA = Root.Instance.Get(buffer.instanceIdA) as b2Box;
                    b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;

                    if (boxA.Id != b2Box.Id || boxB.GetBoxType() is not HitboxType.Hit) continue;

                    // 触发精准闪避回调
                    if (self.functionIndex != 0)
                    {
                        parser.Invoke(self.functionIndex, parser.CancellationToken).Coroutine();
                    }

                    self.Dispose();
                    break;
                }
            }
        }

        public static void Init(this JustEvadeComponent self, int lastFrame, Vector2 center, Vector2 size)
        {
            self.startFrame = BBTimerManager.Instance.SceneTimer().GetNow();
            self.lastFrame = lastFrame;
            self.boxOffset = center;
            self.boxSize = size;
            self.token = new ETCancellationToken();

            // 创建判定框
            b2BoxDef boxDef = new()
            {
                layerType = LayerType.Unit,
                IsTrigger = true,
                Name = "JustEvadeCheckBox",
                Center = self.boxOffset,
                Size = self.boxSize,
                HitboxType = HitboxType.None,
                TriggerEnterId = TriggerEnterType.HandleCallback,
                TriggerStayId = TriggerStayType.HandleCallback,
                TriggerExitId = TriggerExitType.HandleCallback,
                CollisionEnterId = CollisionEnterType.HandleCallback,
                CollisionStayId = CollisionStayType.HandleCallback,
                CollisionExitId = CollisionExitType.HandleCallback
            };
            EventSystem.Instance.Invoke<CreateB2BoxCallback, b2Box>(new CreateB2BoxCallback() { instanceId = self.GetParent<BBParser>().GetParent<Unit>().InstanceId, boxDef = boxDef });
            
            // 窗口期过后销毁组件
            self.DisposeCor().Coroutine();
        }

        private static async ETTask DisposeCor(this JustEvadeComponent self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            await bbTimer.WaitAsync(self.lastFrame, self.token);
            if (self.token.IsCancel()) return;
            
            self.Dispose();
        }
    }
}