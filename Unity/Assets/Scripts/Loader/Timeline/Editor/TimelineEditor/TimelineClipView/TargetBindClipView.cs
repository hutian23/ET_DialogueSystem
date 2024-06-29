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
            menu.AppendAction("Remove keyframe", _ => { RemoveKeyframe(); });
        }

        private void RemoveKeyframe()
        {
            if (ClipInFrame < 0) return;
            EditorWindow.ApplyModify(() => { targetBindClip.TargetKeyframeDict.Remove(ClipInFrame); }, "Remove keyframe");
        }

        private void OnKeyFrameGenerateContent(MeshGenerationContext mgc)
        {
            var paint2D = mgc.painter2D;
            float startFramePos = FramePosMap[BBClip.StartFrame];
            foreach (var kv in targetBindClip.TargetKeyframeDict)
            {
                int currentFrame = BBClip.StartFrame + kv.Key;
                BBTimelineEditorUtility.DrawDiamond(paint2D, FramePosMap[currentFrame] - startFramePos - 1);
            }
        }
    }
}