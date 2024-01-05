using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [CreateAssetMenu(fileName = "Enemy_",menuName = "hello")]
    public class TestConfig : SerializedScriptableObject
    {
        [InfoBox("Hello world")]
        public string enemyName;

        public int a = 10;
        public float b = 10;
        [ShowInInspector]
        [DictionaryDrawerSettings(KeyLabel = "1",ValueLabel = "2")]
        public Dictionary<int, DialogueNode> targets = new();
    }
}
