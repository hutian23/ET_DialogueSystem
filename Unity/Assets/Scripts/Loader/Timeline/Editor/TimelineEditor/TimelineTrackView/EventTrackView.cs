using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class EventTrackView: TimelineTrackView
    {
        private BBEventTrack Track;

        public override void Init(RuntimeTrack track)
        {
            RuntimeTrack = track;
            Track = RuntimeTrack.Track as BBEventTrack;

            int index = EditorWindow.RuntimePlayable.RuntimeTracks.IndexOf(track);
            transform.position = new Vector3(0, index * 40, 0);

            foreach (EventInfo info in Track.EventInfos)
            {
                EventMarkerView markerView = new();
                markerView.Init(this, info);

                markerViews.Add(markerView);
                Add(markerView);
            }
        }

        public override void Refresh()
        {
            foreach (MarkerView markerView in markerViews)
            {
                markerView.Refresh();
            }
        }

        private Vector2 localMousePos;

        protected override void OnPointerDown(PointerDownEvent evt)
        {
            int targetFrame = FieldView.GetClosestFrame(evt.localPosition.x);

            localMousePos = evt.localPosition;

            foreach (MarkerView markerView in markerViews.Where(markerView => markerView.InMiddle(targetFrame)))
            {
                markerView.OnPointerDown(evt);
                break;
            }

            //右键
            if (evt.button == 1)
            {
                // Open menu builder
                m_MenuHandler.ShowMenu(evt);
                evt.StopImmediatePropagation();
            }
        }

        protected override void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Create Event", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                EditorWindow.ApplyModify(() => { Track.EventInfos.Add(new EventInfo() { frame = targetFrame }); }, "Create Event keyframe");
            }, ContainKeyframe(localMousePos.x)? DropdownMenuAction.Status.Hidden : DropdownMenuAction.Status.Normal);
            menu.AppendAction("Delete Event", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                EditorWindow.ApplyModify(() =>
                {
                    EventInfo targetInfo = Track.GetInfo(targetFrame);
                    Track.EventInfos.Remove(targetInfo);
                }, "Delete Event");
            }, ContainKeyframe(localMousePos.x)? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
        }

        private bool ContainKeyframe(float x)
        {
            int frame = FieldView.GetClosestFrame(x);
            return Track.EventInfos.FirstOrDefault(info => info.frame == frame) != null;
        }
    }
}