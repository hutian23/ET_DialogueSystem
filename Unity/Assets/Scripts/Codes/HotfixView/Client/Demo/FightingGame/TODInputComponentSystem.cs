namespace ET.Client
{
    public static class TODInputComponentSystem
    {
        [Invoke(TODTimerInvokeType.CheckInput)]
        public class CheckInputTimer: ATimer<TODInputComponent>
        {
            protected override void Run(TODInputComponent self)
            {
                long ops = FTGHelper.CheckInput();
            }
        }

        public class TODInputComponentAwakeSystem: AwakeSystem<TODInputComponent>
        {
            protected override void Awake(TODInputComponent self)
            {
                self.AddComponent<TODTimerComponent>();
                self.timer = self.GetComponent<TODTimerComponent>().NewFrameTimer(TODTimerInvokeType.CheckInput, self);
            }
        }

        public class TODInputComponentLoadSystem: LoadSystem<TODInputComponent>
        {
            protected override void Load(TODInputComponent self)
            {
                self.GetComponent<TODTimerComponent>().Remove(ref self.timer);
                self.timer = self.GetComponent<TODTimerComponent>().NewFrameTimer(TODTimerInvokeType.CheckInput, self);
            }
        }

        public class TODInputDestroySystem: DestroySystem<TODInputComponent>
        {
            protected override void Destroy(TODInputComponent self)
            {
                self.timer = 0;
            }
        }
    }
}