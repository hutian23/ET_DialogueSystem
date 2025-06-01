using System;
using Testbed.Abstractions;

namespace ET.Client
{
    [FriendOf(typeof(BBTimerManager))]
    [FriendOf(typeof(BBTimerComponent))]
    public static class BBTimerManagerSystem
    {
        public class BBTimerManagerAwakeSystem : AwakeSystem<BBTimerManager>
        {
            protected override void Awake(BBTimerManager self)
            {
                BBTimerManager.Instance = self;
                self.SceneTimer = self.AddChild<BBTimerComponent>().Id;
                self.LateUpdateTimer = self.AddChild<BBTimerComponent>().Id;
                self.Reload();
            }
        }

        // 21462  166666
        public class BBTimerManagerUpdateSystem : UpdateSystem<BBTimerManager>
        {
            protected override void Update(BBTimerManager self)
            {
                self.SceneTimer().SetHertz((int)(Global.Settings.TimeScale * 60));

                // 发生卡顿时，不希望进行追帧，1次Update最多更新1帧
                long now = self._gameTimer.ElapsedTicks;
                long Accumulator = Math.Clamp(now - self.LastTime, 0 , self.SceneTimer().GetFrameLength());
                self.LastTime = now;
                
                self.Step(Accumulator);
            }
        }

        public class BBTimerManagerFrameLateUpdateSystem : FrameLateUpdateSystem<BBTimerManager>
        {
            protected override void FrameLateUpdate(BBTimerManager self)
            {
                self.LateUpdateTimer().Step();
            }
        }

        public class BBTimerManagerReloadSystem : LoadSystem<BBTimerManager>
        {
            protected override void Load(BBTimerManager self)
            {
                self.Reload();
            }
        }

        public static void Step(this BBTimerManager self)
        {
            long Accumulator = self.SceneTimer().GetFrameLength();
            self.Step(Accumulator);
        }

        private static void Step(this BBTimerManager self, long Accumulator)
        {
            BBTimerComponent sceneTimer = self.SceneTimer();
            if (sceneTimer.Hertz == 0) return;
            
            long Dt = sceneTimer.GetFrameLength();
            sceneTimer.Accumulator += Accumulator;
            
            while (sceneTimer.Accumulator >= Dt)
            {
                sceneTimer.Accumulator -= Dt;
             
                //1. FrameUpdate 生命周期事件
                EventSystem.Instance.FrameUpdate();
                
                //2. sceneTimer更新逻辑帧
                sceneTimer.Step();
                Global.Settings.StepCount = sceneTimer.GetNow();
                
                //3. 取出unitTimer更新逻辑帧
                int _Dt = self.instanceIds.Count;
                while (_Dt-- > 0)
                {
                    long instanceId = self.instanceIds.Dequeue();
                    // 组件已销毁，出列
                    if (Root.Instance.Get(instanceId) is not BBTimerComponent bbTimer || bbTimer.IsDisposed) continue;
                    self.instanceIds.Enqueue(instanceId);
                    
                    //SceneTimer逻辑帧帧长是固定的， 永远是 1 / 60 s
                    bbTimer.TimerUpdate(166666);
                }

                //4. 物理层 PreStep PostStep生命周期事件
                b2WorldManager.Instance.Step();
                
                //5. FrameLateUpdate生命周期事件
                EventSystem.Instance.FrameLateUpdate();
            }
        }

        private static void Reload(this BBTimerManager self)
        {
            self._gameTimer.Restart();
            self.LastTime = self._gameTimer.ElapsedTicks;
            
            self.instanceIds.Clear();
            self.SceneTimer().Reload();
            self.LateUpdateTimer().Reload();
        }

        public static void SetHertz(this BBTimerManager self, int hertz)
        {
            self.SceneTimer().SetHertz(hertz);
        }

        public static int GetHertz(this BBTimerManager self)
        {
            return self.SceneTimer().GetHertz();
        }
        
        public static BBTimerComponent SceneTimer(this BBTimerManager self)
        {
            return self.GetChild<BBTimerComponent>(self.SceneTimer);
        }

        public static BBTimerComponent LateUpdateTimer(this BBTimerManager self)
        {
            return self.GetChild<BBTimerComponent>(self.LateUpdateTimer);
        }
        
        //管理timer
        public static void RegistTimer(this BBTimerManager self, long instanceId)
        {
            self.instanceIds.Enqueue(instanceId);
        }

        public static void Pause(this BBTimerManager self, bool pause)
        {
            if (pause)
            {
                self._gameTimer.Stop();
            }
            else
            {
                self._gameTimer.Start();
            }
        }
    }
}