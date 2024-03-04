using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class BBAnimComponent: Entity, IAwake, IDestroy
    {
        public long timer;

        public HashSet<GameObject> hitBoxes = new();
    }
}