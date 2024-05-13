﻿using Timeline;
using UnityEngine;

namespace ET.Client
{
    public class CastBox: CastShapeBase
    {
        public BoxInfo info;

#if UNITY_EDITOR
        protected override void Reset()
        {
            info.center = Vector3.zero;
            info.size = Vector3.one;
        }

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
                    Gizmos.color = Color.cyan;
                    break;
                case HitboxType.Squash:
                    Gizmos.color = Color.yellow;
                    break;
                case HitboxType.Throw:
                    Gizmos.color = Color.magenta;
                    break;
            }

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(info.center, info.size);
            Gizmos.color = gizmosColorRecord;
            Gizmos.matrix = gizmosMatrixRecord;
        }
#endif
    }
}