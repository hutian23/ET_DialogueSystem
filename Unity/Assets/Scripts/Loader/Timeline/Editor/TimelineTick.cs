using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineTick : VisualElement,ISelection
    {
        public new class UxmlFactory : UxmlFactory<TimelineTick,UxmlTraits>
        {
            
        }

        public TimelineTick()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineTick");
            visualTree.CloneTree(this);
            this.AddToClassList("timelineTick");
        }
        
        public VisualElement ContentContainer { get; }
        public List<ISelectable> SelectionElements { get; }
        public List<ISelectable> Selections { get; }
        public void AddToSelection(ISelectable selectable)
        {
        }

        public void RemoveFromSelection(ISelectable selectable)
        {
        }

        public void ClearSelection()
        {
        }
    }
}