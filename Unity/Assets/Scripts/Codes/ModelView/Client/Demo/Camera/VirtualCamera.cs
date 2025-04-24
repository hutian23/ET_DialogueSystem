using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(VirtualCameraManager))]
    public class VirtualCamera : Entity, IAwake, IDestroy
    {
        public GameObject gameObject;
    }
}