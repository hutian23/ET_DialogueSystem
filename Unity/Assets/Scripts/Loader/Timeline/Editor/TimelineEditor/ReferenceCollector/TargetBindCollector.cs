using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline.Editor
{
    [TimelineGenerate]
    public class TargetBindCollector: MonoBehaviour
    {
        [ReadOnly]
        public string targetBindName;
    }
}