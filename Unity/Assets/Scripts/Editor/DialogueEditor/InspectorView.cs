using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public class InspectorView: VisualElement
    {
        public new class UxmlFactory: UxmlFactory<InspectorView, UxmlTraits>
        {
        }

        private Editor editor;

        public void UpdateSelection(List<DialogueNode> nodeList)
        {
            this.Clear();
            UnityEngine.Object.DestroyImmediate(this.editor);
            InspectorDataView dataView = ScriptableObject.CreateInstance<InspectorDataView>();
            dataView.datas = nodeList.ToHashSet();

            this.editor = Editor.CreateEditor(dataView);
            IMGUIContainer container = new(() => { this.editor.OnInspectorGUI(); });
            Add(container);
        }
    }
}