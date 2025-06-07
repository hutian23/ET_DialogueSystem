using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using UnityEngine;
using ET.Event;

namespace ET.Client
{
    public static class SceneBoxHandlerSystem
    {
        [FriendOf(typeof(b2Box))]
        public class SceneBoxHandlerAwakeSystem : AwakeSystem<SceneBoxHandler>
        {
            protected override void Awake(SceneBoxHandler self)
            {
                //1. 生成SceneBox对应的刚体
                Unit unit = self.GetParent<Unit>();
                self.unitId = unit.InstanceId;
                b2Body sceneBody = b2WorldManager.Instance.CreateBody(unit.InstanceId, new BodyDef() { BodyType = BodyType.StaticBody });
                GameObject _World = unit.GetComponent<GameObjectComponent>().GameObject;

                foreach (b2BoxCollider2D box2D in _World.GetComponentsInChildren<b2BoxCollider2D>())
                {
                    BoxInfo info = box2D.info;

                    //2. b2Box管理夹具
                    b2Box b2Box = sceneBody.AddBox(info.boxName);

                    //3. b2Box初始化
                    b2Box.LayerType = info.layerType;
                    b2Box.TagType = info.tagType;
                    b2Box.IsTrigger = info.isTrigger;
                    b2Box.Name = info.boxName;
                    b2Box.Center = info.center.ToVector2();
                    b2Box.Size = info.size.ToVector2();
                    b2Box.HitboxType = info.hitboxType;
                    b2Box.TriggerEnterId = TriggerEnterType.HandleCallback;
                    b2Box.TriggerStayId = TriggerStayType.HandleCallback;
                    b2Box.TriggerExitId = TriggerExitType.HandleCallback;
                    b2Box.CollisionEnterId = CollisionEnterType.HandleCallback;
                    b2Box.CollisionStayId = CollisionStayType.HandleCallback;
                    b2Box.CollisionExitId = CollisionExitType.HandleCallback;
                    
                    //4. 生成夹具
                    PolygonShape shape = new();
                    shape.SetAsBox(b2Box.Size.X / 2f, b2Box.Size.Y / 2f, b2Box.Center, 0f);
                    FixtureDef fixtureDef = new() { Shape = shape, Density = 1.0f, Friction = 0f, UserData = b2Box.InstanceId };
                    b2Box.fixture = sceneBody.CreateFixture(fixtureDef);
                    b2Box.fixtureDef = fixtureDef;
                }
            }
        }

        public class SceneBoxHandlerDestroySystem : DestroySystem<SceneBoxHandler>
        {
            protected override void Destroy(SceneBoxHandler self)
            {
                b2WorldManager.Instance.DestroyBody(self.unitId);   
            }
        }
    }
}