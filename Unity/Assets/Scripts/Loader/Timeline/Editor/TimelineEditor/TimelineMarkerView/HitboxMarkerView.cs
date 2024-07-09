namespace Timeline.Editor
{
    public class HitboxMarkerView: MarkerView
    {
        // private readonly VisualElement MarkerView;
        public HitboxKeyframe keyframe => keyframeBase as HitboxKeyframe;
       
        public override void Select()
        {
            base.Select();
            //Open Inspector
            HitboxMarkerInspectorData inspectorData = new(keyframe);
            inspectorData.InspectorAwake(FieldView);
            TimelineInspectorData.CreateView(FieldView.ClipInspector, inspectorData);
        }
    }
}