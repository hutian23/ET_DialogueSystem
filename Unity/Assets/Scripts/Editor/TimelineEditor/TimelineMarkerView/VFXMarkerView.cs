namespace Timeline.Editor
{
    public class VFXMarkerView : MarkerView
    {
        private VFXKeyFrame keyFrame => this.keyframeBase as VFXKeyFrame;
    }
}