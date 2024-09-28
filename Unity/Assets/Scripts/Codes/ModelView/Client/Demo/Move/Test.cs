using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class Test : Entity,IAwake,IDestroy, IFixedUpdate
    {
        public AnimationCurve curve;
        
    }
}