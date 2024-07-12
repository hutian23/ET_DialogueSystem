using UnityEditor;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class AnimationClipView: TimelineClipView
    {
        private UnityEngine.AnimationClip AnimationClip => (BBClip as BBAnimationClip).animationClip;
        

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
    }
}