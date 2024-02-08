using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    // 管理特效gameobject，例如 Obejction!! Hold It!!
    [ComponentOf(typeof (DialogueComponent))]
    public class EffectManager: Entity, IAwake, IDestroy, ILoad
    {
        public Dictionary<string, GameObject> dic = new();

        public GameObject parent;
    }
}