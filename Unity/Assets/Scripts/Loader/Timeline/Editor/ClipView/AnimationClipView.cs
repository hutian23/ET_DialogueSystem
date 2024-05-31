using UnityEditor;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class AnimationClipView: TimelineClipView
    {
        private UnityEngine.AnimationClip AnimationClip => (BBClip as BBAnimationClip).animationClip;

        public AnimationClipView()
        {
            // m_Content.generateVisualContent += OnKeyFrameGenerateContent;
        }

        protected override void MenuBuilder(DropdownMenu menu)
        {
            base.MenuBuilder(menu);
            menu.AppendAction("Open AnimationClip", _ =>
            {
                AnimationWindow animationWindow = UnityEditor.EditorWindow.GetWindow<AnimationWindow>();
                animationWindow.animationClip = AnimationClip;
                animationWindow.Show();
            });
        }

        // private void OnKeyFrameGenerateContent(MeshGenerationContext mgc)
        // {
        //     var paint2D = mgc.painter2D;
        //
        //     float startFramePos = FramePosMap[BBClip.StartFrame];
        //
        //     if (AnimationClip == null) return;
        //
        //     var keyframeSet = BBTimelineEditorUtility.GetAnimationKeyframes(AnimationClip);
        //     foreach (var keyframe in keyframeSet)
        //     {
        //         int currentFrame = BBClip.StartFrame + keyframe;
        //         if (!FramePosMap.ContainsKey(currentFrame)) continue;
        //         BBTimelineEditorUtility.DrawDiamond(paint2D, FramePosMap[currentFrame] - startFramePos - 1);
        //     }
        // }
    }
}