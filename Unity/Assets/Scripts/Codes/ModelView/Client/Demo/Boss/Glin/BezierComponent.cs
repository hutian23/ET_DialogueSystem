using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class BezierComponent : Entity, IAwake, IDestroy, IPreStep, IGizmosUpdate
    {
        public float speed;
        public float percent;
        public Vector2 startPoint;
        public Vector2 midPoint;
        public Vector2 endPoint;
    }
}