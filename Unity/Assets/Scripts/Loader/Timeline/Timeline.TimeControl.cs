using System;

namespace Timeline
{
    public class TimeSpeedTrack: Track
    {
#if UNITY_EDITOR
        public override Type ClipType => typeof (TimeSpeedClip);
#endif
    }

    public class TimeSpeedClip: Clip
    {
        [ShowInInspector, OnValueChanged("ReBindTimeline")]
        public float TargetSpeed;

        protected double m_ChangedValue;

        public override void Bind()
        {
            base.Bind();
            m_ChangedValue = 0;
        }

        public override void UnBind()
        {
            base.UnBind();
            Timeline.TimelinePlayer.PlaySpeed -= m_ChangedValue;
        }

        public override void Evaluate(float deltaTime)
        {
            double targetSpeed = 0;
            TargetTime = Time += deltaTime;

            if (Time < StartTime)
            {
                targetSpeed = 0;
            }
            else if (StartTime <= Time && Time <= EndTime)
            {
                float selfTime = Time - StartTime;
                float remainTime = EndTime - Time;

                if (selfTime < EaseInTime)
                {
                    targetSpeed = selfTime / EaseInTime;
                }
                else if (remainTime < EaseOutTime)
                {
                    targetSpeed = remainTime / EaseOutTime;
                }
                else
                {
                    targetSpeed = 1;
                }
            }
            else if (Time > EndTime)
            {
                targetSpeed = 0;
            }

            targetSpeed *= TargetSpeed;
            targetSpeed = Math.Round(targetSpeed, 2);

            double deltaSpeed = targetSpeed - m_ChangedValue;
            Timeline.TimelinePlayer.PlaySpeed += deltaSpeed;
            m_ChangedValue += deltaSpeed;
        }

#if UNITY_EDITOR
        public override ClipCapabilities Capabilities => base.Capabilities | ClipCapabilities.Mixable | ClipCapabilities.Resizeable;
        public TimeSpeedClip(Track track, int frame): base(track, frame)
        {
        }
#endif
    }
}