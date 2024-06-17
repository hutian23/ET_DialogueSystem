using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public sealed class BehaviorClipView: VisualElement
    {
        public new class UxmlFactory: UxmlFactory<BehaviorClipView, UxmlTraits>
        {
        }

        private DropdownMenuManipulator dropManipulator;

        public BehaviorClipView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/BehaviorClipView");
            visualTree.CloneTree(this);

            var styleSheet = Resources.Load<StyleSheet>($"Style/BehaviorClipView");
            styleSheets.Add(styleSheet);
            AddToClassList("BehaviorClipView");

            dropManipulator = new DropdownMenuManipulator((menu) => { menu.AppendAction("Hello world", _ => { Debug.LogWarning("Hello world"); }); },
                MouseButton.RightMouse);
            this.AddManipulator(dropManipulator);
        }
    }
}