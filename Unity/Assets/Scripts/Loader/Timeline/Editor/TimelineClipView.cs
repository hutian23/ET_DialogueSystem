using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineClipView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TimelineClipView,UxmlTraits> {}

        protected string m_DefaultVisualTreeGuid = "";
        protected virtual string VisualTreeGuid => m_DefaultVisualTreeGuid;
        
        public bool Selected { get; private set; }
        public bool Hoverd { get; private set; }
        public ISelection SelectionContainer { get; set; }
        public ClipCapabilities Capabilities;

        public TimelineFieldView FieldView;
        public TimelineEditorWindow EditorWindow;
        public Timeline Timeline;
        public Dictionary<int, float> FramePosMap => FieldView.FramePosMap;
        public Clip Clip { get; private set; }
        public TimelineTrackView TrackView { get; private set; }

        public int StartFrame => Clip.StartFrame;
        public int EndFrame => Clip.EndFrame;
        public int OtherEaseInFrame => Clip.OtherEaseInFrame;
        public int OtherEaseOutFrame => Clip.OtherEaseOutFrame;
        public int SelfEaseInFrame => Clip.SelfEaseInFrame;
        public int SelfEaseOutFrame => Clip.SelfEaseOutFrame;
        public int EaseInFrame => Clip.EaseInFrame;
        public int EaseOutFrame => Clip.EaseOutFrame;
        public int ClipInFrame => Clip.ClipInFrame;
        public int WidthFrame => EndFrame - StartFrame;
        
        
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