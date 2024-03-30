using UnityEngine.Audio;
using UnityEngine.Playables;

namespace Timeline
{
    public class AudioTrack: Track
    {
        public int PlayableIndex { get; protected set; }
        
    }

    public class TimelineAudioTrackPlayable: PlayableBehaviour
    {
        public AudioTrack Track { get; private set; }
        public Playable Output { get; private set; }
        public Playable Handle { get; private set; }
        public AudioMixerPlayable MixerPlayable { get; private set; }
        public Timeline Timeline => Track.Timeline;
    }
}