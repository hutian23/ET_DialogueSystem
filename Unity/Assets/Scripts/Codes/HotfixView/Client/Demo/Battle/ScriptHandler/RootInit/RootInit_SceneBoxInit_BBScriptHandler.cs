using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;
using UnityEngine;

namespace ET.Client
{
    public class RootInit_SceneBoxInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SceneBoxInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            GameObject _World = unit.GetComponent<GameObjectComponent>().GameObject;

            //1. 添加组件
            // unit.AddComponent<SceneBoxHandler>(); // 处理碰撞事件
            b2Body sceneBody = unit.AddComponent<b2Body, BodyDef>(new BodyDef(){ BodyType =  BodyType.StaticBody});
            
            //2. SceneBox 作为SceneBody的Fixture
            foreach (b2BoxCollider2D box2D in _World.GetComponentsInChildren<b2BoxCollider2D>())
            {
                PolygonShape shape = new();
                shape.SetAsBox(box2D.info.size.x / 2, box2D.info.size.y / 2, new System.Numerics.Vector2(box2D.info.center.x, box2D.info.center.y), 0f);
                FixtureDef fixtureDef = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0.0f,
                    UserData = new FixtureData()
                    {
                        InstanceId = sceneBody.InstanceId, //代表SceneBox
                        Name = box2D.info.boxName,
                        Type = FixtureType.Default,
                        LayerMask = LayerType.Ground,
                        IsTrigger = box2D.IsTrigger,
                        UserData = box2D.info,
                        TriggerStayId = TriggerStayType.HandleCallback,
                        CollisionStayId = TriggerStayType.HandleCallback
                    }
                };
                sceneBody.CreateFixture(fixtureDef);
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}