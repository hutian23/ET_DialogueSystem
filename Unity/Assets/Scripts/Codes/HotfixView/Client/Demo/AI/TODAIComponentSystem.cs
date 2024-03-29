﻿using System;

namespace ET.Client
{
    [FriendOf(typeof (TODAIComponent))]
    public static class TODAIComponentSystem
    {
        [Invoke(TimerInvokeType.TOD_AITimer)]
        [FriendOf(typeof (TODAIComponent))]
        public class TODAITimer: ATimer<TODAIComponent>
        {
            protected override void Run(TODAIComponent self)
            {
                try
                {
                    self.Simulate();
                    self.Check();
                }
                catch (Exception e)
                {
                    Log.Error($"ai behavior error ! : current order : {self.order} \n {e}");
                }
            }
        }

        public class TODAIComponentAwakeSystem: AwakeSystem<TODAIComponent>
        {
            protected override void Awake(TODAIComponent self)
            {
                self.AddComponent<Buffer>();
            }
        }
        
        public class TODAIComponentLoadSystem : LoadSystem<TODAIComponent>
        {
            protected override void Load(TODAIComponent self)
            {
                self.Cancel();
                TimerComponent.Instance.Remove(ref self.AITimer);
                self.AITimer = TimerComponent.Instance.NewFrameTimer(TimerInvokeType.TOD_AITimer, self);
            }
        }

        public static void AILoad(this TODAIComponent self, AIBehaviorConfig config)
        {
            self.Cancel();
            self.config = config;
            TimerComponent.Instance.Remove(ref self.AITimer);
            self.AITimer = TimerComponent.Instance.NewFrameTimer(TimerInvokeType.TOD_AITimer, self);
        }
        
        private static void Simulate(this TODAIComponent self)
        {
            for (int i = 0; i < self.config.checkers.Count; i++)
            {
                CheckerConfig config = self.config.checkers[i];
                CheckerHandler checkerHandler = TODEventSystem.Instance.GetChecker(config.checkerName);
                if (checkerHandler == null)
                {
                    Log.Error($"not found aCheckerHandler~: {config.name}");
                    continue;
                }

                checkerHandler.Execute(self.GetParent<Unit>(), config);
            }
        }

        private static void Check(this TODAIComponent self)
        {
            for (int i = 0; i < self.config.AIBehaviors.Count; i++)
            {
                var behavior = self.config.AIBehaviors[i];
                BehaviorHandler behaviorHandler = TODEventSystem.Instance.GetBehavior(behavior.behaviorName);
                if (behaviorHandler == null)
                {
                    Log.Error($"不存在behaviorHandler: {behavior.behaviorName}");
                    continue;
                }

                Unit ai = self.GetParent<Unit>();
                int ret = behaviorHandler.Check(ai, behavior.config);
                if (ret != 0)
                {
                    continue;
                }

                //相同行为
                if (self.order == i + 1)
                {
                    return;
                }

                self.Cancel();
                ETCancellationToken token = new();
                self.Token = token;
                self.order = i + 1;

                behaviorHandler.Handler(ai, behavior.config, token).Coroutine();
                return;
            }
        }

        public static void Cancel(this TODAIComponent self)
        {
            self.Token?.Cancel();
            self.Token = null;
            self.order = 0;
        }
    }
}