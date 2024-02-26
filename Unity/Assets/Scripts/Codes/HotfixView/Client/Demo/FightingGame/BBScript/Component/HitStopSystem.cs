using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (HitStop))]
    public static class HitStopSystem
    {
        public class HitStopAwakeSystem: AwakeSystem<HitStop, int>
        {
            protected override void Awake(HitStop self, int waitFrame)
            {
                TODTimerComponent timerComponent = self.GetParent<DialogueComponent>().GetComponent<TODTimerComponent>();
                self.waitFrame = waitFrame;
                self.preTimeScale = timerComponent.GetTimeScale();
                timerComponent.SetTimeScale(0f);
            }
        }
        
        public class HitStopUpdateSystem: UpdateSystem<HitStop>
        {
            protected override void Update(HitStop self)
            {
                self.deltaTimeReminder += Time.deltaTime * 1000;
                float frameLength = Mathf.Round(1000 / (60 * self.timeScale));
                int num = (int)(self.deltaTimeReminder / frameLength);
                self.deltaTimeReminder -= num * frameLength;
                self.frameCounter += num;

                if (self.frameCounter >= self.waitFrame)
                {
                    self.GetParent<DialogueComponent>().GetComponent<TODTimerComponent>()?.SetTimeScale(self.preTimeScale);
                    self.Dispose();
                }
            }
        }
    }
}