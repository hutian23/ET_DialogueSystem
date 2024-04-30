using System;

namespace Timeline
{
    public class BBTrackAttribute : Attribute
    {
        public string TrackName;

        public BBTrackAttribute(string trackName)
        {
            TrackName = trackName;
        }
    }
}