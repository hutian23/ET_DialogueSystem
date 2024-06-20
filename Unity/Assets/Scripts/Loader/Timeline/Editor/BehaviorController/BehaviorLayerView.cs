using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class BehaviorLayerView : VisualElement
    {
        public new class UxmlFactory: UxmlFactory<BehaviorLayerView,UxmlTraits>
        {
        }

        public BehaviorLayerView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/BehaviorLayerView");
            visualTree.CloneTree(this);
        }
    }
}