using UnityEngine;

namespace ET
{
    [CreateAssetMenu(menuName = "ScriptableObject/CheckerConfig", fileName = "CheckerConfig_")]
    public class CheckerConfig : BaseScriptableObject
    {
        public string checkerName;
        [TextArea]
        public string Description;
    }
}