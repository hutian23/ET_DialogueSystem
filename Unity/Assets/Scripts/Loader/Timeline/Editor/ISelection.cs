using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public interface ISelection
    {
        public VisualElement ContentContainer { get; }
        public List<ISelectable> Elements { get; }
        protected List<ISelectable> Selections { get; }

        protected void AddToSelection(ISelectable selectable);
        protected void RemoveFromSelection(ISelectable selectable);
        protected void ClearSelection();
    }
}