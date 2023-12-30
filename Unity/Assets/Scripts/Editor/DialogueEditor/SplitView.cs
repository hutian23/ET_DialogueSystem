using UnityEngine.UIElements;

namespace ET.Client
{
    public class SplitView: TwoPaneSplitView
    {
        public new class UxmlFactory: UxmlFactory<SplitView,UxmlTraits>
        {
        }

        public SplitView()
        {
            this.orientation = TwoPaneSplitViewOrientation.Horizontal;
        }
    }
}