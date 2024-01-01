using UnityEditor;
using UnityEngine.UIElements;

namespace ET.Client
{
    public class CheckerView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<CheckerView,UxmlTraits>
        { }

        private Editor editor;
        
        public CheckerView()
        {
        }
        
        public void UpdateSelection(DialogueNodeView nodeView)
        {
            this.Clear();
            UnityEngine.Object.DestroyImmediate(this.editor);
            if(nodeView.node.Config == null) return;
            this.editor = Editor.CreateEditor(nodeView.node.Config);
            IMGUIContainer container = new(() =>
            {
                this.editor.OnInspectorGUI();
            });
            Add(container);
        }
    }
}