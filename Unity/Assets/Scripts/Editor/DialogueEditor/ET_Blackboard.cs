using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ET.Client
{
    public sealed class ET_Blackboard: Blackboard
    {
        public ET_Blackboard(GraphView _graphView): base(_graphView)
        {
            var header = this.Q("header");
            header.style.height = new StyleLength(50);
            scrollable = true;

            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            PopulateMenuItem();
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            this.contentContainer.style.height = this.layout.height - 50;
        }

        private void PopulateMenuItem()
        {
            this.addItemRequested = _blackboard =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Int"), false, AddVariable);
                menu.ShowAsContext();
            };
        }

        private void AddVariable()
        {
            var field = new BlackboardField { text = "test", typeText = "String" };
            var sa = new BlackboardRow(field, new IntegerField());
            Add(sa);
        }
    }
}