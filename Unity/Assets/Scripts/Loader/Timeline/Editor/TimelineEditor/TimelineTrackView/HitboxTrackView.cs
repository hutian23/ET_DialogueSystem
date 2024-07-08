using System.Collections.Generic;
using System.Linq;
using ET;
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

            foreach (HitboxKeyframe keyframe in Track.Keyframes)
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

        private Vector2 localMousePos;

        protected override void OnPointerDown(PointerDownEvent evt)
        {
            int targetFrame = FieldView.GetClosestFrame(evt.localPosition.x);

            localMousePos = evt.localPosition;

            foreach (HitboxMarkerView markerView in markerViews.Where(markerView => markerView.InMiddle(targetFrame)))
            {
                markerView.OnPointerDown(evt);
            }

            //右键
            if (evt.button == 1)
            {
                // Open menu builder
                m_MenuHandler.ShowMenu(evt);
                evt.StopImmediatePropagation();
            }
        }

        #region Menu

        protected override void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Create Keyframe", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                EditorWindow.ApplyModify(() => { Track.Keyframes.Add(new HitboxKeyframe() { frame = targetFrame }); }, "Create Hitbox Keyframe");
            }, ContainKeyframe(localMousePos.x)? DropdownMenuAction.Status.Hidden : DropdownMenuAction.Status.Normal);
            menu.AppendAction("Remove keyframe", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                HitboxKeyframe keyframe = Track.GetKeyframe(targetFrame);
                EditorWindow.ApplyModify(() => { Track.Keyframes.Remove(keyframe); }, "Remove hitbox keyframe");
            }, ContainKeyframe(localMousePos.x)? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
            menu.AppendAction("Copy keyframe", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                HitboxKeyframe copyFrame = MongoHelper.Clone(Track.GetKeyframe(targetFrame));
                BBTimelineSettings.GetSettings().CopyTarget = copyFrame;
            }, ContainKeyframe(localMousePos.x)? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
            menu.AppendAction("Paste keyframe", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                //copy target not a hitboxkeyframe
                HitboxKeyframe targetKeyframe = BBTimelineSettings.GetSettings().CopyTarget as HitboxKeyframe;
                if (targetKeyframe == null)
                {
                    return;
                }

                if (ContainKeyframe(localMousePos.x))
                {
                    Debug.LogError($"already contain keyframe in : {targetFrame}");
                    return;
                }

                HitboxKeyframe cloneKeyframe = MongoHelper.Clone(targetKeyframe);
                cloneKeyframe.frame = targetFrame;
                EditorWindow.ApplyModify(() => { Track.Keyframes.Add(cloneKeyframe); }, "Paste Hitbox Keyframe");
            }, CanPaste()? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
        }

        private bool CanPaste()
        {
            //copy target not a hitboxkeyframe
            HitboxKeyframe targetKeyframe = BBTimelineSettings.GetSettings().CopyTarget as HitboxKeyframe;
            return targetKeyframe != null;
        }

        private bool ContainKeyframe(float x)
        {
            int frame = FieldView.GetClosestFrame(x);
            return Track.GetKeyframe(frame) != null;
        }

        #endregion

        #region Move marker

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

            foreach (HitboxMarkerView marker in moveMarkers)
            {
                marker.Move(deltaFrame);
            }

            //Resize frameMap
            int maxFrame = int.MinValue;
            foreach (HitboxMarkerView marker in moveMarkers)
            {
                if (marker.keyframe.frame >= maxFrame)
                {
                    maxFrame = marker.keyframe.frame;
                }
            }

            FieldView.ResizeTimeField(maxFrame);

            //检查重叠
            foreach (HitboxMarkerView marker in moveMarkers)
            {
                marker.InValid = GetMarkerMoveValid(marker);
            }

            Refresh();
            FieldView.DrawFrameLine(startFrame);
        }

        private bool GetMarkerMoveValid(HitboxMarkerView markerView)
        {
            foreach (HitboxMarkerView view in markerViews)
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
            foreach (ISelectable selection in FieldView.Selections)
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
                foreach (HitboxMarkerView markerView in moveMarkers)
                {
                    markerView.ResetMove(deltaFrame);
                }

                if (InValid)
                {
                    EditorWindow.ApplyModify(() =>
                    {
                        foreach (HitboxMarkerView markerView in moveMarkers)
                        {
                            markerView.Move(deltaFrame);
                        }
                    }, "Move hitbox markers");
                }
            }

            Refresh();
            FieldView.DrawFrameLine();
        }

        #endregion
    }
}