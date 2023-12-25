using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class TODMoveComponent: Entity, IAwake, IDestroy, IUpdate
    {
        public Rect Collider; //注意这里position 代表了碰撞箱的offset
        public Vector2 position;
        public Vector2 Speed;
        public LayerMask CollideMask;

        public bool IsActor;
        public ActorHandler actorHandler; // 只有这部分的逻辑需要多态
        public SolidHandler solidHandler;
    }
}