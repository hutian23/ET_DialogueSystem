using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class BulletComponent : Entity, IAwake ,IDestroy
    {
        public GameObject GameObject;
    }
}