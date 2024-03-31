using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineTrackView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TimelineTrackView,UxmlTraits> {}

        protected bool m_Selected;

        public ISelection SelectionContainer
        {
            get; set; 
        }
        
        // public Timeline Timeline => Edi
        public bool IsSelectable()
        {
            throw new System.NotImplementedException();
        }

        public bool HitTest(Vector2 localPoint)
        {
            throw new System.NotImplementedException();
        }

        public void Select(VisualElement selectionContainer, bool additive)
        {
            throw new System.NotImplementedException();
        }

        public void Unselect(VisualElement selectionContainer)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSelected(VisualElement selectionContainer)
        {
            throw new System.NotImplementedException();
        }
    }
}