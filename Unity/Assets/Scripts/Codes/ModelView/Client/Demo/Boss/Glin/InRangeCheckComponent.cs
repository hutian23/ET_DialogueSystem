using UnityEngine;

namespace ET.Client 
{
    [ComponentOf(typeof(BBParser))]
    public class InRangeCheckComponent : Entity, IAwake, IDestroy, IPostStep, IGizmosUpdate
    {
        public bool inRange;
        public Vector2 center;
        public float radius;

        public int inRangeCallbackIndex;
    }
}