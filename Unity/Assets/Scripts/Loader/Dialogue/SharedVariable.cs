using Sirenix.OdinInspector;

namespace ET.Client
{
    [ReadOnly]
    public class SharedVariable
    {
        public string name;

        [HideReferenceObjectPicker]
        public object value;

        public static SharedVariable Create(string name, object value)
        {
            var variable = ObjectPool.Instance.Fetch<SharedVariable>();
            variable.name = name;
            variable.value = value;
            return variable;
        }

        public void Recycle()
        {
            name = string.Empty;
            value = default;
            ObjectPool.Instance.Recycle(this);
        }
    }
}