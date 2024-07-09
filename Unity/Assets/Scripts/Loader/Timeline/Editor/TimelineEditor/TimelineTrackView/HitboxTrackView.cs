using System.Linq;
using ET;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public sealed class HitboxTrackView: TimelineTrackView
    {
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
                evt.StopImmediatePropagation();
                return;
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
    }
}