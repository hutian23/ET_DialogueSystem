using ET;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TargetBindClipView: TimelineClipView
    {
        private BBTargetBindClip targetBindClip => BBClip as BBTargetBindClip;
        private int ClipInFrame => FieldView.GetCurrentTimeLocator() - targetBindClip.StartFrame;

        public TargetBindClipView()
        {
            m_Content.generateVisualContent += OnKeyFrameGenerateContent;
        }

        protected override void MenuBuilder(DropdownMenu menu)
        {
            base.MenuBuilder(menu);
            menu.AppendAction("Add keyframe", _ => { AddKeyframe(); });
            menu.AppendAction("Remove keyframe", _ => { RemoveKeyframe(); });
            menu.AppendAction("Copy keyframe", _ => { Copyframe(); });
            menu.AppendAction("Paste keyframe", _ => { PasteKeyframe(); });
        }

        private void AddKeyframe()
        {
            if (ClipInFrame < 0) return;
            if (targetBindClip.TargetKeyframeDict.ContainsKey(ClipInFrame))
            {
                Debug.LogError($"already has targetbind keyframe: {ClipInFrame}");
                return;
            }

            EditorWindow.ApplyModify(() => { targetBindClip.TargetKeyframeDict.Add(ClipInFrame, new TargetKeyframe()); }, "Add targetBind keyframe");
        }

        private void RemoveKeyframe()
        {
            if (ClipInFrame < 0) return;
            EditorWindow.ApplyModify(() => { targetBindClip.TargetKeyframeDict.Remove(ClipInFrame); }, "Remove keyframe");
        }

        private void Copyframe()
        {
            if (!targetBindClip.TargetKeyframeDict.TryGetValue(ClipInFrame, out TargetKeyframe keyframe)) return;
            var cloneKeyframe = MongoHelper.Clone(keyframe);
            BBTimelineSettings.GetSettings().CopyTarget = cloneKeyframe;
        }

        private void PasteKeyframe()
        {
            EditorWindow.ApplyModify(() =>
            {
                TargetKeyframe keyframe = BBTimelineSettings.GetSettings().CopyTarget as TargetKeyframe;
                targetBindClip.TargetKeyframeDict.Remove(ClipInFrame);
                targetBindClip.TargetKeyframeDict.Add(ClipInFrame, keyframe);
            }, "Paste keyframe`");
        }

        private void OnKeyFrameGenerateContent(MeshGenerationContext mgc)
        {
            var paint2D = mgc.painter2D;
            paint2D.strokeColor = Color.white;
            paint2D.BeginPath();

            float startFramePos = FramePosMap[BBClip.StartFrame];
            foreach (var kv in targetBindClip.TargetKeyframeDict)
            {
                int currentFrame = BBClip.StartFrame + kv.Key;
                paint2D.MoveTo(new Vector2(FramePosMap[currentFrame] - startFramePos, 0));
                paint2D.LineTo(new Vector2(FramePosMap[currentFrame] - startFramePos, 15));
            }

            paint2D.Stroke();
        }
    }
}