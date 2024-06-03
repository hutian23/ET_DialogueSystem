using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ET.Client
{
    public class BBTestManager: SerializedMonoBehaviour
    {
        [HideInInspector]
        public long instanceId;

        [HideInInspector, OdinSerialize]
        public Dictionary<string, int> dropdownDict = new();

        [HideInInspector]
        public int currentOrder;
    }
}