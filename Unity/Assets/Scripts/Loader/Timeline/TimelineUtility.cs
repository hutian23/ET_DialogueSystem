using System;
using UnityEngine;

namespace Timeline
{
    public delegate void Evaluate(float deltaTime);

    public static class TimelineUtility
    {
        public const int FrameRate = 60;

        private static float MinEvaluateDeltaTime => 1f / FrameRate;

        public static void Lerp(float targetTime, float deltaTime, Evaluate evaluateSplitDeltaTime, ref float lastTime)
        {
            //正向 or 逆向?
            int direction = deltaTime > 0? 1 : -1;
            if (Mathf.Abs(deltaTime) > MinEvaluateDeltaTime)
            {
                while (Math.Abs(lastTime - targetTime) > 0.01f)
                {
                    float splitDeltaTime = direction * MinEvaluateDeltaTime;
                    splitDeltaTime = direction == 1? Mathf.Min(splitDeltaTime, targetTime - lastTime) : Mathf.Max(splitDeltaTime, targetTime - lastTime);
                    
                    evaluateSplitDeltaTime(splitDeltaTime);
                    lastTime += splitDeltaTime;
                }
            }
            else
            {
                evaluateSplitDeltaTime(deltaTime);
                lastTime += deltaTime;
            }
        }
    }
}