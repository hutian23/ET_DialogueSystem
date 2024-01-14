using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (Unit))]
    public class EmojiComponent: Entity, IAwake, IDestroy, ILoad
    {
        public GameObject emoji;
    }
}