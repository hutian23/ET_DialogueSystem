using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineClipView : VisualElement ,ISelectable
    {
        public new class UxmlFactory : UxmlFactory<TimelineClipView,UxmlTraits> {}

        protected string m_DefaultVisualTreeGuid = "";
        protected virtual string VisualTreeGuid => m_DefaultVisualTreeGuid;
        
        public bool Selected { get; private set; }
        public bool Hoverd { get; private set; }
        public ISelection SelectionContainer { get; set; }
        // public ClipCapabilities Capabilities => 
        
        // public Dictionary<int,float> FramePosMap => FieldVie
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