using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace ET.Client
{
    [CustomEditor(typeof (DialogueViewComponent))]
    public class DialogueViewComponentEditor: OdinEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DialogueViewComponent component = target as DialogueViewComponent;
            if (component == null) return;
            component.OpenWindow ??= () => { DialogueEditor.OpenWindow(component); };
        }
    }
}