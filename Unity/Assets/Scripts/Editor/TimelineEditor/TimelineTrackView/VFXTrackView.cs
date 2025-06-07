using System.Linq;
using ET;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class VFXTrackView : TimelineTrackView
    {
        private VFXTrack vfxTrack => this.Track as VFXTrack;
        
        public override void Init(BBTrack track)
        {
            Track = track;

            int index = EditorWindow.BBTimeline.Tracks.IndexOf(track);
            transform.position = new Vector3(0, index * 40, 0);

            foreach (VFXKeyFrame keyFrame in vfxTrack.KeyFrames)
            {
                VFXMarkerView markerView = new();
                markerView.Init(this, keyFrame);

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
                EditorWindow.ApplyModify(() => { vfxTrack.KeyFrames.Add(new VFXKeyFrame() { frame = targetFrame }); }, "Create Hitbox Keyframe");
            }, ContainKeyframe(localMousePos.x)? DropdownMenuAction.Status.Hidden : DropdownMenuAction.Status.Normal);
            menu.AppendAction("Remove keyframe", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                VFXKeyFrame keyframe = vfxTrack.GetKeyFrame(targetFrame);
                EditorWindow.ApplyModify(() => { vfxTrack.KeyFrames.Remove(keyframe); }, "Remove hitbox keyframe");
            }, ContainKeyframe(localMousePos.x)? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
            menu.AppendAction("Copy keyframe", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                VFXKeyFrame copyFrame = MongoHelper.Clone(vfxTrack.GetKeyFrame(targetFrame));
                BBTimelineSettings.GetSettings().CopyTarget = copyFrame;
            }, ContainKeyframe(localMousePos.x)? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
            menu.AppendAction("Paste keyframe", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                //copy target not a eyFrame
                VFXKeyFrame targetKeyframe = BBTimelineSettings.GetSettings().CopyTarget as VFXKeyFrame;
                if (targetKeyframe == null)
                {
                    return;
                }

                if (ContainKeyframe(localMousePos.x))
                {
                    Debug.LogError($"already contain keyframe in : {targetFrame}");
                    return;
                }

                VFXKeyFrame cloneKeyframe = MongoHelper.Clone(targetKeyframe);
                cloneKeyframe.frame = targetFrame;
                EditorWindow.ApplyModify(() => { vfxTrack.KeyFrames.Add(cloneKeyframe); }, "Paste Hitbox Keyframe");
            }, CanPaste()? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
        }

        private bool CanPaste()
        {
            //copy target not a hitBoxKeyFrame
            HitboxKeyframe targetKeyframe = BBTimelineSettings.GetSettings().CopyTarget as HitboxKeyframe;
            return targetKeyframe != null;
        }

        private bool ContainKeyframe(float x)
        {
            int frame = FieldView.GetClosestFrame(x);
            return vfxTrack.GetKeyFrame(frame) != null;
        }

        #endregion
    }
}