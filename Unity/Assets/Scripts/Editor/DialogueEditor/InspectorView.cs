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
            ScrollView scrollView = new();
            IMGUIContainer container = new(() => { this.editor.OnInspectorGUI(); });
            scrollView.Add(container);
            Add(scrollView);
        }

        public void UpdateVaraibleView(List<SharedVariable> variables)
        {
            this.Clear();
            UnityEngine.Object.DestroyImmediate(this.editor);
            ShareVariableView variableView = ScriptableObject.CreateInstance<ShareVariableView>();
            variableView.SharedVariables = variables;
            editor = Editor.CreateEditor(variableView);

            ScrollView scrollView = new();
            IMGUIContainer container = new(() => { this.editor.OnInspectorGUI(); });
            scrollView.Add(container);
            Add(scrollView);
        }
    }
}