using Sirenix.OdinInspector;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineInspectorData: SerializedScriptableObject
    {
        [HideLabel]
        [HideReferenceObjectPicker]
        public IShowInInspector obj;

        public static TimelineInspectorData CreateView(VisualElement parent, IShowInInspector _obj)
        {
            if (_obj == null) return null;
            TimelineInspectorData inspectorData = CreateInstance<TimelineInspectorData>();
            inspectorData.obj = _obj;

            var editor = UnityEditor.Editor.CreateEditor(inspectorData);
            IMGUIContainer container = new(() => { editor.OnInspectorGUI(); });
            parent.Clear();
            parent.Add(container);

            return inspectorData;
        }
    }

    public interface IShowInInspector
    {
        public void InspectorUpdate(TimelineFieldView fieldView);
    }
}