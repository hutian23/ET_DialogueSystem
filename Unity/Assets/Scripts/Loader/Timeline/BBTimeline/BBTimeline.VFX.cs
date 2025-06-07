using System;
using System.Collections.Generic;
using UnityEngine;

namespace Timeline
{
    [BBTrack("VFX")]
    [Color(127, 214, 253)]
    [IconGuid("348da3c2f85477f4594cabc88bc48a84")]
    public class VFXTrack: BBTrack
    {
        public List<VFXKeyFrame> KeyFrames = new();

        public override Type RuntimeTrackType => typeof (RuntimeVFXTrack);

        public VFXKeyFrame GetKeyFrame(int targetFrame)
        {
            foreach (VFXKeyFrame keyframe in KeyFrames)
            {
                if (keyframe.frame == targetFrame) return keyframe;
            }

            return null;
        }
        
        public VFXKeyFrame GetClosestKeyframe(int targetFrame)
        {
            int closestFrame = -1;
            foreach (VFXKeyFrame keyFrame in KeyFrames)
            {
                if (keyFrame.frame == targetFrame)
                {
                    closestFrame = keyFrame.frame;
                    break;
                }

                if (keyFrame.frame < targetFrame && targetFrame - keyFrame.frame < targetFrame - closestFrame)
                {
                    closestFrame = keyFrame.frame;
                }
            }

            return closestFrame == -1? null : GetKeyFrame(closestFrame);
        }

#if UNITY_EDITOR
        public override int GetMaxFrame()
        {
            int max = 1;
            foreach (VFXKeyFrame keyframe in KeyFrames)
            {
                if (keyframe.frame >= max)
                {
                    max = keyframe.frame;
                }
            }

            return max;
        }
#endif
    }

    public class VFXKeyFrame : BBKeyframeBase
    {
        public Vector2 Position;
    }

    public class RuntimeVFXTrack: RuntimeTrack
    {
        public RuntimeVFXTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
            
        }

        public override void UnBind()
        {
        }

        public override void SetTime(int targetFrame)
        {
        }
    }
}