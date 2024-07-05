using System.Collections.Generic;
using ET;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class HitboxClipView: TimelineClipView
    {
        private BBHitboxClip hitboxClip => BBClip as BBHitboxClip;
        private int clipInFrame => FieldView.GetCurrentTimeLocator() - hitboxClip.StartFrame;

        public HitboxClipView()
        {
            m_Content.generateVisualContent += OnKeyFrameGenerateContent;

            m_Content.Add(new TimelineMarkerView());
        }

        protected override void MenuBuilder(DropdownMenu menu)
        {
            base.MenuBuilder(menu);
            menu.AppendAction("Add Hitbox Keyframe", _ => { Addkeyframe(); });
            menu.AppendAction("Remove Hitbox Keyframe", _ => { RemoveKeyframe(); });
            menu.AppendAction("Copy Hitbox Keyframe", _ => { CopyKeyframe(); },
                _ => hitboxClip.boxInfoDict.ContainsKey(clipInFrame)?
                        DropdownMenuAction.Status.Normal :
                        DropdownMenuAction.Status.Hidden);
            menu.AppendAction("Paste Hitbox Keyframe", _ => { PasteKeyframe(); }
                , _ => BBTimelineSettings.GetSettings().CopyTarget is List<BoxInfo>?
                        DropdownMenuAction.Status.Normal :
                        DropdownMenuAction.Status.Hidden);
        }

        private void Addkeyframe()
        {
            if (clipInFrame < 0) return;
            if (hitboxClip.boxInfoDict.ContainsKey(clipInFrame))
            {
                Debug.LogError($"already has hitbox key frame: {clipInFrame}");
                return;
            }

            EditorWindow.ApplyModify(() => { hitboxClip.boxInfoDict.Add(clipInFrame, new List<BoxInfo>()); }, "Add hitbox keyframe");
        }

        private void RemoveKeyframe()
        {
            if (clipInFrame < 0) return;
            EditorWindow.ApplyModify(() => { hitboxClip.boxInfoDict.Remove(clipInFrame); }, "Remove hitbox keyframe");
        }

        private void CopyKeyframe()
        {
            if (!hitboxClip.boxInfoDict.TryGetValue(clipInFrame, out var infos)) return;

            var cloneInfos = MongoHelper.Clone(infos);
            BBTimelineSettings.GetSettings().CopyTarget = cloneInfos;
        }

        private void PasteKeyframe()
        {
            List<BoxInfo> infos = MongoHelper.Clone(BBTimelineSettings.GetSettings().CopyTarget) as List<BoxInfo>;
            if (infos == null) return;

            EditorWindow.ApplyModify(() =>
            {
                hitboxClip.boxInfoDict.Remove(clipInFrame);
                hitboxClip.boxInfoDict.Add(clipInFrame, infos);
            }, "Paste key frame");
        }

        private void OnKeyFrameGenerateContent(MeshGenerationContext mgc)
        {
            var paint2D = mgc.painter2D;

            float startFramePos = FramePosMap[BBClip.StartFrame];
            foreach (var kv in hitboxClip.boxInfoDict)
            {
                int currentFrame = BBClip.StartFrame + kv.Key;
                BBTimelineEditorUtility.DrawDiamond(paint2D, FramePosMap[currentFrame] - startFramePos - 1);
            }
        }
    }
}