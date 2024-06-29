using Sirenix.OdinInspector;

namespace Timeline.Editor
{
    [HideMonoScript]
    public class BehaviorActiveObject: SerializedScriptableObject
    {
        [HideReferenceObjectPicker]
        [HideLabel]
        public System.Object ActiveObject;
    }
}