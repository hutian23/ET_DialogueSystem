using Sirenix.OdinInspector;

namespace ET.Client
{
    public class InspectorDataView : SerializedScriptableObject
    {
        public test222 select;

        public InspectorDataView(test222 ins)
        {
            this.select = ins;
        }
    }
}