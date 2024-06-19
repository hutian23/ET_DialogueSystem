using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public sealed class BehaviorClipView: Node
    {
        public BehaviorClip BehaviorClip;

        public Port Input;
        public Port Output;

        public BehaviorClipView()
        {
            Input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof (bool));
            Input.portName = "Input";
            Output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof (bool));
            Output.portName = "Output";

            inputContainer.Add(Input);
            outputContainer.Add(Output);

            ProgressBar bar = new();
            bar.style.height = 13;
            bar.style.display = DisplayStyle.None;
            contentContainer.Add(bar);
        }

        public void Init(BehaviorClip behaviorClip)
        {
            //Timeline
            BehaviorClip = behaviorClip;
            title = behaviorClip.Title;
            viewDataKey = behaviorClip.viewDataKey;

            Rect oldPos = GetPosition();
            oldPos.position = behaviorClip.ClipPos;
            SetPosition(oldPos);

            //Selection
            RegisterCallback<PointerDownEvent>(_ => { BBTimelineSettings.GetSettings().SetActiveObject(behaviorClip); });
        }

        public void Refresh()
        {
            if (BehaviorClip == null) return;
            title = BehaviorClip.Title;
        }
    }
}