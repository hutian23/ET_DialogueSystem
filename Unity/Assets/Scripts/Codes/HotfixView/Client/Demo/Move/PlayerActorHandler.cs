using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(TODMoveComponent))]
    public class PlayerActorHandler : ActorHandler
    {
        public override void UpdateCollideX(TODMoveComponent moveComponent, float distX)
        {
            (float moved, RaycastHit2D hit) = moveComponent.MoveXStepWithCollide(distX);
            moveComponent.position += Vector2.right * moved;

            //移动到目标距离了
            if (Mathf.Abs(distX - moved) <= 0.001f) return;

            float tempDist = distX - moved;
            //尝试修正Y轴 <---- 前方有阻挡物，不能y轴修正
            if (moveComponent.CorrectX(tempDist)) return;

            moveComponent.Speed.x = 0;
            if (!hit || hit.collider == null) return;

        }

        public override void UpdateCollideY(TODMoveComponent moveComponent, float distY)
        {
            (float moved, RaycastHit2D hit) = moveComponent.MoveYStepWithCollide(distY);
            if (Mathf.Abs(distY - moved) <= 0.001f) return;

            float tempDist = distY - moved;
            if (moveComponent.CorrectY(tempDist)) return;
                
            moveComponent.Speed.y = 0;
            if (!hit || hit.collider == null) return;

        }
    }
}