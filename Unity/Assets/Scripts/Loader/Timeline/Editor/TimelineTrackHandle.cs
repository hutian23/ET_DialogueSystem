using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineTrackHandle : VisualElement,ISelectable
    {
        public new class UxmlFactory : UxmlFactory<TimelineTrackHandle,UxmlTraits> { }
        public TextField NameField { get; private set; }
        public VisualElement Icon { get; private set; }
        
        public TimelineTrackView TrackView { get; private set; }
        public TimelineEditorWindow EditorWindow;
        public TimelineFieldView FieldView;
        public Track Track;
        public Timeline Timeline;

        private DropdownMenuHandler MenuHandler;
        private float TopOffset = 5;
        private float YminOffset = -77;
        private float Interval = 40;

        public TimelineTrackHandle()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("VisualTree/TimelineTrackHandle");
            visualTree.CloneTree(this);
            pickingMode = PickingMode.Ignore;
        }

        public TimelineTrackHandle(TimelineTrackView trackView): this()
        {
            TrackView = trackView;
            // TrackView.OnS
        }
        
        public ISelection SelectionContainer { get; set; }
        public bool IsSelectable()
        {
            return false;
        }

        public void Select()
        {
            throw new System.NotImplementedException();
        }

        public void UnSelect()
        {
            throw new System.NotImplementedException();
        }

        public bool IsSelected()
        {
            throw new System.NotImplementedException();
        }
    }
}