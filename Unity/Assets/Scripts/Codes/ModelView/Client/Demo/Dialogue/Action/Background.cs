using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(DialogueComponent))]
    public class Background : Entity,IAwake,IDestroy, ILoad
    {
        public GameObject background;
    }
}