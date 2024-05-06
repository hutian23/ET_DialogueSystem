using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class HitboxClipView: TimelineClipView
    {
        protected override void MenuBuilder(DropdownMenu menu)
        {
            base.MenuBuilder(menu);
            menu.AppendAction("Hello world", _ => { Debug.LogWarning("hello world"); });
        }

        public override void PopulateInspector()
        {
            Debug.LogWarning("Hitbox inspector");
        }
    }
}