using UnityEditor;
using UnityEngine.UIElements;

namespace ET.Client
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory: UxmlFactory<InspectorView, UxmlTraits>{}

        private Editor editor;

        public InspectorView()
        {
        }
        
        public void UpdateSelection(DialogueNodeView nodeView)
        {
            // this.Clear();
            // UnityEngine.Object.DestroyImmediate(this.editor);
            // this.editor = Editor.CreateEditor(nodeView.node);
            // IMGUIContainer container = new(() =>
            // {
            //     this.editor.OnInspectorGUI();
            // });
            // Add(container);
        }
    }
}