using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class MarkerClipView: TimelineClipView
    {
        public List<TimelineMarkerView> MarkerViews = new();

        public MarkerClipView()
        {
            this.Add(new TimelineMarkerView());
        }
        
        protected override void MenuBuilder(DropdownMenu menu)
        {
        }
    }
}