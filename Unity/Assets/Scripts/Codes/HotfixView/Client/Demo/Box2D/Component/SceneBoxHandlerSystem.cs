using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public static class SceneBoxHandlerSystem
    {
        public class SceneBoxHandlerAwakeSystem : AwakeSystem<SceneBoxHandler>
        {
            protected override void Awake(SceneBoxHandler self)
            {
                //1. 生成SceneBox
                Unit unit = self.GetParent<Unit>();
                b2Body sceneBody = b2WorldManager.Instance.CreateBody(unit.InstanceId, new BodyDef() { BodyType = BodyType.StaticBody });
                GameObject _World = unit.GetComponent<GameObjectComponent>().GameObject;

                //2. 
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
                            InstanceId = sceneBody.InstanceId,
                            Name = box2D.info.boxName,
                            Type = FixtureType.Default,
                            LayerType = LayerType.Ground,
                            IsTrigger = box2D.IsTrigger,
                            UserData = box2D.info,
                            TriggerEnterId =  TriggerEnterType.HandleCallback,
                            TriggerStayId = TriggerStayType.HandleCallback,
                            TriggerExitId =  TriggerExitType.HandleCallback,
                            CollisionEnterId = CollisionEnterType.HandleCallback,
                            CollisionStayId = CollisionStayType.HandleCallback,
                            CollisionExitId = CollisionExitType.HandleCallback,
                        }
                    };
                    sceneBody.CreateFixture(fixtureDef);
                }
                self.unitId = unit.InstanceId;
            }
        }
        
        public class SceneBoxHandlerDestroySystem : DestroySystem<SceneBoxHandler>
        {
            protected override void Destroy(SceneBoxHandler self)
            {
                self.info = default;
                b2WorldManager.Instance.DestroyBody(self.unitId);
            }
        }
        
        // public class SceneBoxHandlerGizmosUpdateSystem : GizmosUpdateSystem<SceneBoxHandler>
        // {
        //     protected override void GizmosUpdate(SceneBoxHandler self)
        //     {
        //         GameObject go = self.GetParent<BBParser>().GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
        //
        //         foreach (b2BoxCollider2D box in go.GetComponentsInChildren<b2BoxCollider2D>())
        //         {
        //             PolygonShape shape = new();
        //             shape.SetAsBox(box.info.size.x / 2, box.info.size.y / 2, box.info.center.ToVector2(), 0f);
        //             b2WorldManager.Instance.DrawShape(shape, System.Numerics.Vector2.Zero, 0f, Color.White);
        //         }
        //     }
        // }
        
        // public class SceneBoxHandlerPostStepSystem : PostStepSystem<SceneBoxHandler>
        // {
        //     protected override void PosStepUpdate(SceneBoxHandler self)
        //     {
        //         //TriggerEnter事件
        //         int count = self.TriggerEnterQueue.Count;
        //         while (count-- > 0)
        //         {
        //             CollisionInfo info = self.TriggerEnterQueue.Dequeue();
        //             self.HandleEvent(info, 0);
        //         }
        //
        //         //TriggerStay事件
        //         count = self.TriggerStayQueue.Count;
        //         while (count-- > 0)
        //         {
        //             CollisionInfo info = self.TriggerStayQueue.Dequeue();
        //             self.HandleEvent(info, 1);
        //         }
        //
        //         //TriggerExit事件
        //         count = self.TriggerExitQueue.Count;
        //         while (count-- > 0)
        //         {
        //             CollisionInfo info = self.TriggerExitQueue.Dequeue();
        //             self.HandleEvent(info, 2);
        //         }
        //         
        //         //CollisionEnter
        //         count = self.CollisionEnterQueue.Count;
        //         while (count-- > 0)
        //         {
        //             CollisionInfo info = self.CollisionEnterQueue.Dequeue();
        //             self.HandleEvent(info, 3);
        //         }
        //         
        //         //CollisionStay
        //         count = self.CollisionStayQueue.Count;
        //         while (count-- > 0)
        //         {
        //             CollisionInfo info = self.CollisionStayQueue.Dequeue();
        //             self.HandleEvent(info, 4);
        //         }
        //         
        //         //CollisionExit
        //         count = self.CollisionExitQueue.Count;
        //         while (count -- > 0)
        //         {
        //             CollisionInfo info = self.CollisionExitQueue.Dequeue();
        //             self.HandleEvent(info, 5);
        //         }
        //     }
        // }
        //
        // /// <summary>
        // /// 
        // /// </summary>
        // /// <param name="self"></param>
        // /// <param name="info"></param>
        // /// <param name="type">type = 1, triggerEnter; type = 2, triggerStay; type = 3, triggerExit; </param>
        // private static void HandleEvent(this SceneBoxHandler self, CollisionInfo info, int type)
        // {
        //     BBParser parser = self.GetParent<BBParser>();
        //
        //     //检查是否存在回调
        //     string groupName = info.dataA.Name;
        //     if (!parser.ContainGroup(groupName)) return;
        //     
        //     string funcName = string.Empty;
        //     switch (type)
        //     {
        //         case 0:
        //             funcName = "TriggerEnter";
        //             break;
        //         case 1:
        //             funcName = "TriggerStay"; 
        //             break;
        //         case 2:
        //             funcName = "TriggerExit";
        //             break;
        //         case 3:
        //             funcName = "CollisionEnter";
        //             break;
        //         case 4:
        //             funcName = "CollisionStay";
        //             break;
        //         case 5:
        //             funcName = "CollisionExit";
        //             break;
        //     }
        //     if (!parser.ContainFunction(groupName, funcName)) return;
        //     
        //     //调用回调(同步!!!)
        //     parser.RegistParam("CollisionInfo", info);
        //     parser.Invoke(parser.GetFunctionPointer(groupName, funcName), parser.CancellationToken).Coroutine();
        //     parser.TryRemoveParam("CollisionInfo");
        // }
    }
}