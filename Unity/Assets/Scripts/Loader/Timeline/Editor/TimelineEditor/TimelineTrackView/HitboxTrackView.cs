using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public sealed class HitboxTrackView: TimelineTrackView
    {
        private readonly List<HitboxMarkerView> markerViews = new();
        private BBHitboxTrack Track;

        public override void Init(RuntimeTrack track)
        {
            RuntimeTrack = track;
            Track = RuntimeTrack.Track as BBHitboxTrack;

            int index = EditorWindow.RuntimePlayable.RuntimeTracks.IndexOf(track);
            transform.position = new Vector3(0, index * 40, 0);

            foreach (var keyframe in Track.Keyframes)
            {
                HitboxMarkerView markerView = new();
                markerView.Init(this, keyframe);

                markerViews.Add(markerView);
                Add(markerView);
                FieldView.SelectionElements.Add(markerView); // selection
            }
        }

        public override void Refresh()
        {
            foreach (HitboxMarkerView markerView in markerViews)
            {
                markerView.Refresh();
            }
        }

        protected override void OnPointerDown(PointerDownEvent evt)
        {
            int targetFrame = FieldView.GetClosestFrame(evt.localPosition.x);
            foreach (HitboxMarkerView markerView in markerViews)
            {
                if (!markerView.InMiddle(targetFrame))
                {
                    continue;
                }

                markerView.OnPointerDown(evt);
            }

            //右键
            if (evt.button == 1)
            {
                // Open menu builder
                evt.StopImmediatePropagation();
            }
        }

        private int m_startMoveMarkerFrame;

        public void MarkerStartMove(HitboxMarkerView markerView)
        {
            m_startMoveMarkerFrame = markerView.keyframe.frame;
        }

        public void MoveMarkers(float deltaPosition)
        {
            int startFrame = int.MaxValue;
            List<HitboxMarkerView> moveMarkers = new List<HitboxMarkerView>();
            foreach (ISelectable selectable in FieldView.Selections)
            {
                if (selectable is HitboxMarkerView markerView)
                {
                    moveMarkers.Add(markerView);
                    if (markerView.keyframe.frame < startFrame)
                    {
                        startFrame = markerView.keyframe.frame;
                    }
                }
            }

            if (moveMarkers.Count == 0)
            {
                return;
            }

            int targetStartFrame = FieldView.GetClosestFrame(FieldView.FramePosMap[startFrame] + deltaPosition);
            targetStartFrame = Mathf.Clamp(targetStartFrame, FieldView.CurrentMinFrame, FieldView.CurrentMaxFrame);

            int deltaFrame = targetStartFrame - startFrame;

            foreach (var marker in moveMarkers)
            {
                marker.Move(deltaFrame);
            }

            //Resize frameMap
            int maxFrame = int.MinValue;
            foreach (var marker in moveMarkers)
            {
                if (marker.keyframe.frame >= maxFrame)
                {
                    maxFrame = marker.keyframe.frame;
                }
            }

            FieldView.ResizeTimeField(maxFrame);

            //检查重叠
            foreach (var marker in moveMarkers)
            {
                marker.InValid = GetMarkerMoveValid(marker);
            }

            Refresh();
            FieldView.DrawFrameLine(startFrame);
        }

        private bool GetMarkerMoveValid(HitboxMarkerView markerView)
        {
            foreach (var view in markerViews)
            {
                if (view == markerView)
                {
                    continue;
                }

                if (view.keyframe.frame == markerView.keyframe.frame)
                {
                    return false;
                }
            }

            return true;
        }

        public void ApplyMarkerMove()
        {
            int startFrame = int.MaxValue;
            bool InValid = true;

            List<HitboxMarkerView> moveMarkers = new();
            foreach (var selection in FieldView.Selections)
            {
                if (selection is not HitboxMarkerView markerView) continue;

                //override with other marker
                if (!markerView.InValid)
                {
                    InValid = false;
                }

                if (markerView.keyframe.frame <= startFrame)
                {
                    startFrame = markerView.keyframe.frame;
                }

                moveMarkers.Add(markerView);
            }

            int deltaFrame = startFrame - m_startMoveMarkerFrame;

            if (deltaFrame != 0)
            {
                //Reset position
                foreach (var markerView in moveMarkers)
                {
                    markerView.ResetMove(deltaFrame);
                }

                if (InValid)
                {
                    EditorWindow.ApplyModify(() =>
                    {
                        foreach (var markerView in moveMarkers)
                        {
                            markerView.Move(deltaFrame);
                        }
                    }, "Move hitbox markers");
                }
            }

            FieldView.DrawFrameLine();
        }
    }
}