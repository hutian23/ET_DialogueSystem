using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class GameObjectComponent: Entity, IAwake, IDestroy, ILoadCached
    {
        public GameObject GameObject { get; set; }
    }
}