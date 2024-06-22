using Sirenix.OdinInspector;

namespace ET.Client
{
    [ReadOnly]
    public class SharedVariable
    {
        public string name;

        [HideReferenceObjectPicker]
        public object value;
    }
}