using ET;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class ParticleClipView: TimelineClipView
    {
        private BBParticleClip particleClip => BBClip as BBParticleClip;
        private int clipInFrame => FieldView.GetCurrentTimeLocator() - particleClip.StartFrame;

        public ParticleClipView()
        {
            m_Content.generateVisualContent += OnKeyFrameGenerateContent;
        }

        protected override void MenuBuilder(DropdownMenu menu)
        {
            base.MenuBuilder(menu);
            menu.AppendAction("Add particle keyframe", _ => { AddKeyframe(); });
            menu.AppendAction("Remove particle keyframe", _ => { RemoveKeyframe(); });
            menu.AppendAction("Copy particle keyframe", _ => { CopyKeyframe(); },
                _ => particleClip.keyframeDict.ContainsKey(clipInFrame)?
                        DropdownMenuAction.Status.Normal :
                        DropdownMenuAction.Status.Hidden);
            menu.AppendAction("Paste particle keyframe", _ => { PasteKeyframe(); },
                _ => BBTimelineSettings.GetSettings().CopyTarget is ParticleKeyframe?
                        DropdownMenuAction.Status.Normal :
                        DropdownMenuAction.Status.Hidden);
        }

        private void AddKeyframe()
        {
            if (clipInFrame < 0) return;
            if (particleClip.keyframeDict.ContainsKey(clipInFrame))
            {
                Debug.LogError($"already has particle key frame: {clipInFrame}");
                return;
            }

            EditorWindow.ApplyModify(() => { particleClip.keyframeDict.Add(clipInFrame, new ParticleKeyframe()); }, "Add particle keyframe");
        }

        private void RemoveKeyframe()
        {
            if (clipInFrame < 0) return;
            EditorWindow.ApplyModify(() => { particleClip.keyframeDict.Remove(clipInFrame); }, "Remove particle keyframe");
        }

        private void CopyKeyframe()
        {
            if (!particleClip.keyframeDict.TryGetValue(clipInFrame, out var keyframe)) return;
            var cloneKeyframe = MongoHelper.Clone(keyframe);
            BBTimelineSettings.GetSettings().CopyTarget = cloneKeyframe;
        }

        private void PasteKeyframe()
        {
            EditorWindow.ApplyModify(() =>
            {
                ParticleKeyframe keyframe = BBTimelineSettings.GetSettings().CopyTarget as ParticleKeyframe;
                particleClip.keyframeDict.Remove(clipInFrame);
                particleClip.keyframeDict.Add(clipInFrame, keyframe);
            }, "Paste particle keyframe");
        }

        private void OnKeyFrameGenerateContent(MeshGenerationContext mgc)
        {
            var paint2D = mgc.painter2D;
            paint2D.strokeColor = Color.white;
            paint2D.BeginPath();

            float startFramePos = FramePosMap[BBClip.StartFrame];
            foreach (var kv in particleClip.keyframeDict)
            {
                int currentFrame = BBClip.StartFrame + kv.Key;
                paint2D.MoveTo(new Vector2(FramePosMap[currentFrame] - startFramePos, 0));
                paint2D.LineTo(new Vector2(FramePosMap[currentFrame] - startFramePos, 15));
            }

            paint2D.Stroke();
        }
    }
}