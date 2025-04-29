using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(BounceCheckComponent))]
    public static class BounceCheckComponentSystem
    {
        public class BounceCheckAwakeSystem : AwakeSystem<BounceCheckComponent, Vector2, Vector2>
        {
            protected override void Awake(BounceCheckComponent self, Vector2 offset, Vector2 size)
            {
                self.offsetX = offset.X;
                self.offsetY = offset.Y;
                self.sizeX = size.X;
                self.sizeY = size.Y;
                self.token = new ETCancellationToken();
                
                self.GenerateCheckBox();
                self.BounceCheckCor().Coroutine();
            }
        }
        
        public class BounceCheckDestroySystem : DestroySystem<BounceCheckComponent>
        {
            protected override void Destroy(BounceCheckComponent self)
            {
                self.offsetX = 0;
                self.offsetY = 0;
                self.sizeX = 0;
                self.sizeY = 0;
                self.DestroyCheckBox();
                self.token.Cancel();
            }
        }

        private static void GenerateCheckBox(this BounceCheckComponent self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            //1. 夹具定义
            PolygonShape shape = new();
            shape.SetAsBox(self.sizeX / 2, self.sizeY / 2, new Vector2(self.offsetX * body.GetFlip(), self.offsetY), 0f);
            FixtureDef fixtureDef = new()
            {
                Shape = shape,
                Density = 1.0f,
                Friction = 0f,
                UserData = new FixtureData()
                {
                    InstanceId = body.InstanceId,
                    Name = "BounceCheckBox",
                    Type = FixtureType.Default,
                    LayerMask = LayerType.Unit,
                    IsTrigger = true,
                    UserData = new BoxInfo()
                    {
                        boxName = "BounceCheckBox",
                        hitboxType = HitboxType.Other,
                        center = new Vector2(self.offsetX, self.offsetY).ToUnityVector2(),
                        size = new Vector2(self.sizeX, self.sizeY).ToUnityVector2()
                    },
                    TriggerStayId = TriggerStayType.TriggerEvent
                }
            };
            
            //2. 生成夹具
            self.checkBox = body.CreateFixture(fixtureDef);
        }

        private static void DestroyCheckBox(this BounceCheckComponent self)
        {
            if(self.token.IsCancel()) return;
            
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            
            if (self.checkBox == null || body == null) return;
            
            body.DestroyFixture(self.checkBox);
        }

        private static async ETTask BounceCheckCor(this BounceCheckComponent self)
        {
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            BBParser parser = unit.GetComponent<BBParser>();
            
            bool ret = false;
            while (true)
            {
                await postStepTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;
                
                // 取出缓冲区的碰撞信息，逐个检测
                Queue<CollisionInfo> infoQueue = b2Unit.TriggerBuffer;
                int count = infoQueue.Count;
                while (count-- > 0)
                {
                    CollisionInfo info = infoQueue.Dequeue();
                    infoQueue.Enqueue(info);

                    // BounceCheckBox和墙体发生重叠
                    if (!info.fixtureA.Equals(self.checkBox) || info.dataB.LayerMask is not LayerType.Ground || info.dataB.InstanceId == 0) continue;

                    parser.TryRemoveParam("Flag_Bounce");
                    parser.RegistParam("Flag_Bounce", true);
                    ret = true;
                    break;
                }

                if (ret) break;
            }
            
            self.Dispose();
        }
    }
}