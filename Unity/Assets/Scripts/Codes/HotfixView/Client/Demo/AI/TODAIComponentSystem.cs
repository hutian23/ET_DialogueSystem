namespace ET.Client
{
    [FriendOf(typeof(TODAIComponent))]
    public static class TODAIComponentSystem
    {
        [Invoke(TimerInvokeType.TOD_AITimer)]
        public class TODAITimer : ATimer<TODAIComponent>
        {
            protected override void Run(TODAIComponent self)
            {
            }
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
            // for(int i = 0;i<self.config.)
        }
    }
}