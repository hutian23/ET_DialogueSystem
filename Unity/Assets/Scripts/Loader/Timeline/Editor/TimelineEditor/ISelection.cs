using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public interface ISelection
    {
        public VisualElement ContentContainer { get; }
        public List<ISelectable> SelectionElements { get; }
        public List<ISelectable> Selections { get; }

        public void AddToSelection(ISelectable selectable);
        public void RemoveFromSelection(ISelectable selectable);
        public void ClearSelection();
    }
}