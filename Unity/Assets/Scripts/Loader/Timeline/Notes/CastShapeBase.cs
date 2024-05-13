﻿using UnityEngine;

namespace ET.Client
{
    public abstract class CastShapeBase : MonoBehaviour
    {
#if UNITY_EDITOR
        protected readonly Color m_gizmosColor = Color.cyan;
        protected virtual void Reset()
        {
        }

        protected virtual void OnDrawGizmos()
        {
            
        }
#endif
    }
}