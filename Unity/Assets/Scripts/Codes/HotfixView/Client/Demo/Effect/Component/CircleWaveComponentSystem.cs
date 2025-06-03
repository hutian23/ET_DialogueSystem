using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(CircleWaveComponent))]
    public static class CircleWaveComponentSystem
    {
        public class CircleWaveComponentAwakeSystem : AwakeSystem<CircleWaveComponent, float, float, int>
        {
            protected override void Awake(CircleWaveComponent self, float waveWidth, float waveSpeed, int totalTick)
            {
                self.waveWidth = waveWidth;
                self.waveSpeed = waveSpeed;
                self.progress = 0;
                self.totalTick = totalTick;
                self.currentTick = 0;
                self.PropertyBlock = new MaterialPropertyBlock();
                self.CircleChange(0, 0);
                // self.token = new ETCancellationToken();
                // self.CircleWaveCor().Coroutine();
            }
        }

        // private static async ETTask CircleWaveCor(this CircleWaveComponent self)
        // {
        //     BBTimerComponent bbTimer = self.GetParent<BBParser>().GetParent<Unit>().GetComponent<BBTimerComponent>();
        //     
        //     while (self.currentTick ++ < self.totalTick)
        //     {
        //         float progress = (float)self.currentTick / self.totalTick;
        //         float curSpeed = Mathf.Lerp(self.waveSpeed, 0f, progress);
        //         self.progress += curSpeed * ScriptHelper.FrameLength;
        //         self.CircleChange(self.waveWidth, self.progress);
        //
        //         await bbTimer.WaitFrameAsync(self.token);
        //         if (self.token.IsCancel()) return;
        //     }
        //     
        //     self.Dispose();
        // }
        
        public class CircleWaveComponentFrameUpdateSystem : FrameUpdateSystem<CircleWaveComponent>
        {
            protected override void FrameUpdate(CircleWaveComponent self)
            {
                if (self.currentTick++ >= self.totalTick)
                {
                    self.Dispose();
                    return;
                }
                
                float progress = (float)self.currentTick / self.totalTick;
                float curSpeed = Mathf.Lerp(self.waveSpeed, 0f, progress);
                self.progress += curSpeed * ScriptHelper.FrameLength;
                
                self.CircleChange(self.waveWidth, self.progress);
            }
        }

        private static void CircleChange(this CircleWaveComponent self, float waveWidth, float progress)
        {
            SpriteRenderer renderer = self.GetParent<BBParser>()
                    .GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<SpriteRenderer>();
            renderer.GetPropertyBlock(self.PropertyBlock);
            self.PropertyBlock.SetFloat("_Width", waveWidth);
            self.PropertyBlock.SetFloat("_Progress", progress);
            renderer.SetPropertyBlock(self.PropertyBlock);
        }

        public class CircleWaveComponentDestroySystem : DestroySystem<CircleWaveComponent>
        {
            protected override void Destroy(CircleWaveComponent self)
            {
                self.waveWidth = 0f;
                self.waveSpeed = 0f;
                self.progress = 0f;
                self.totalTick = 0;
                self.currentTick = 0;
                self.CircleChange(0, 0);
                self.PropertyBlock.Clear();
                // self.token.Cancel();
            }
        }
    }
}