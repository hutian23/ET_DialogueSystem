namespace ET.Client
{
    public static class TODInputComponentSystem
    {
        [Invoke(TODTimerInvokeType.CheckInput)]
        public class CheckInputTimer: TODTimer<TODInputComponent>
        {
            protected override void Run(TODInputComponent self)
            {
                long ops = FTGHelper.CheckInput();
                self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgFtg>().Refresh(ops);
            }
        }

        public class TODInputComponentAwakeSystem: AwakeSystem<TODInputComponent>
        {
            protected override void Awake(TODInputComponent self)
            {
                self.ClientScene().GetComponent<UIComponent>().ShowWindow<DlgFtg>();
                TODTimerComponent todTimerComponent = self.AddComponent<TODTimerComponent>();
                self.timer = todTimerComponent.NewOnceTimer(todTimerComponent.GetNow() + 300, TODTimerInvokeType.CheckInput, self);
            }
        }

        public class TODInputComponentLoadSystem: LoadSystem<TODInputComponent>
        {
            protected override void Load(TODInputComponent self)
            {
                self.ClientScene().GetComponent<UIComponent>().UnLoadWindow<DlgFtg>();
                self.ClientScene().GetComponent<UIComponent>().ShowWindow<DlgFtg>();
                
                self.RemoveComponent<TODTimerComponent>();   
                TODTimerComponent timerComponent = self.AddComponent<TODTimerComponent>();
                self.timer = timerComponent.NewFrameTimer(TODTimerInvokeType.CheckInput, self);
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