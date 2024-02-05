using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(DialogueComponent))]
    public class Background : Entity,IAwake,IDestroy
    {
        public GameObject background;
    }
}