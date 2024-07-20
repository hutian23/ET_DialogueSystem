using AABB;
using MongoDB.Bson;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class CollisionTest: MonoBehaviour
    {
        public World world;
        public IBox body;

        public void Start()
        {
        }

        [Button("测试")]
        public void TestCollide()
        {
            world = new World(500, 300);

            //box1
            body = world.Create(0, 0, 20, 20);

            //box2
            world.Create(40, 40, 100, 100);

            Debug.LogWarning(body.Bounds);
            //Try to move the box to (100,200) with a slide movement for every other collided body
            var result = body.Move(80, 80, (_) => CollisionResponses.Touch);

            Debug.LogWarning(result.ToJson());
            Debug.LogWarning(body.Bounds);
            if (result.HasCollided)
            {
                Debug.Log("Body collided");
            }
        }

        public void Update()
        {
        }
    }
}