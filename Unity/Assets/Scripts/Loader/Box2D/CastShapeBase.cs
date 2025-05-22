using UnityEngine;

namespace ET
{
    public abstract class CastShapeBase : MonoBehaviour
    {
#if UNITY_EDITOR
        protected readonly Color m_gizmosColor = Color.cyan;

        protected virtual void OnDrawGizmos()
        {
            
        }
#endif
    }
}