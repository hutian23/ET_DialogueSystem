using ET;
using UnityEngine;

namespace Timeline
{
    public class CastBox: CastShapeBase, ITimelineGenerate
    {
        public BoxInfo info;

#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            Matrix4x4 gizmosMatrixRecord = Gizmos.matrix;
            Color gizmosColorRecord = Gizmos.color;
        
            switch (info.hitboxType)
            {
                case HitboxType.Hit:
                    Gizmos.color = Color.red;
                    break;
                case HitboxType.Hurt:
                    Gizmos.color = Color.green;
                    break;
                case HitboxType.Squash:
                    Gizmos.color = Color.yellow;
                    break;
                case HitboxType.Throw:
                    Gizmos.color = Color.blue;
                    break;
                case HitboxType.Proximity:
                    Gizmos.color = Color.magenta;
                    break;
                case HitboxType.Other:
                    Gizmos.color = Color.white;
                    break;
                case HitboxType.Gizmos:
                    Gizmos.color = Color.white;
                    break;
            }
            
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
            Gizmos.DrawWireCube(info.center, info.size);
            Gizmos.color = gizmosColorRecord;
            Gizmos.matrix = gizmosMatrixRecord;
        }  
#endif
    }
}