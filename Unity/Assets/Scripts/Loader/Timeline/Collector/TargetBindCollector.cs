using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline
{
    [TimelineGenerate]
    public class TargetBindCollector: MonoBehaviour
    {
        [ReadOnly]
        public string targetBindName;
    }
}