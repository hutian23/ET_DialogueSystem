using UnityEngine;

namespace Timeline.Editor
{
    public interface ISelectable
    {
        public ISelection SelectionContainer { get; set; }

        /// <summary>
        /// Check if element is selectable
        /// </summary>
        public bool IsSelectable();

        /// <summary>
        /// Check if selection overlaps rectangle
        /// </summary>
        public bool Overlaps(Rect rectangle);

        /// <summary>
        /// Select element
        /// </summary>
        public void Select();

        public void UnSelect();

        public bool IsSelected();
    }

    public interface IShowInspector
    {
        public void InspectorAwake();
        public void InsepctorUpdate();
        public void InspectorDestroy();
    }
}