namespace Timeline.Editor
{
    public class EventMarkerView: MarkerView
    {
        public override void Select()
        {
            base.Select();
            //Open Inspector
            EventInspectorData inspectorData = new(keyframeBase);
            inspectorData.InspectorAwake(FieldView);
            TimelineInspectorData.CreateView(FieldView.ClipInspector, inspectorData);
        }
    }
}