using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class RotationComponent : Entity, IAwake, IDestroy, IPostStep
    {
        public Vector3 EulerAngles;
    }
}