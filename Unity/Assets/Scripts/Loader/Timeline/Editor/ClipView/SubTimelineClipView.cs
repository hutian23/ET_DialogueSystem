using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class SubTimelineClipView: TimelineClipView
    {
        protected override void MenuBuilder(DropdownMenu menu)
        {
            base.MenuBuilder(menu);
            menu.AppendAction("Open Editor Window", evt =>
            {
                TimelineEditorWindow window = ScriptableObject.CreateInstance<TimelineEditorWindow>();
                window.Show();
            });
        }
    }
}