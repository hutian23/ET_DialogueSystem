using ET.Client;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class BehaviorParamView: VisualElement
    {
        public new class UxmlFactory: UxmlFactory<BehaviorParamView, UxmlTraits>
        {
        }

        public SharedVariable variable;

        public BehaviorParamView()
        {
            VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>($"VisualTree/BehaviorParamView");
            visualTreeAsset.CloneTree(this);
            AddToClassList("BehaviorParamView");
        }

        public void Select()
        {
            AddToClassList("Selected");
        }

        public void UnSelect()
        {
            InEditMode(false);
            RemoveFromClassList("Selected");
        }

        public bool InMiddle(Vector2 worldPosition)
        {
            return worldBound.Contains(worldPosition);
        }

        //Edit mode 编辑textField
        public void InEditMode(bool editMode)
        {
            TextField textField = this.Q<TextField>("param-editor-text");
            Label label = this.Q<Label>("param-editor-label");

            textField.style.display = editMode? DisplayStyle.Flex : DisplayStyle.None;
            if (editMode)
            {
                textField.Focus();
            }

            label.style.display = editMode? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}