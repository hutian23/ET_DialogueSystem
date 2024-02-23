using UnityEngine.InputSystem;

namespace ET.Client
{
    public static class TODInputComponentSystem
    {
        [Invoke(TODTimerInvokeType.CheckInput)]
        [FriendOf(typeof (BBInputComponent))]
        [FriendOf(typeof (TODTimerComponent))]
        public class CheckInputTimer: TODTimer<BBInputComponent>
        {
            protected override void Run(BBInputComponent self)
            {
                long ops = FTGHelper.CheckInput();
                if (Gamepad.current.startButton.isPressed)
                {
                    EventSystem.Instance.Load();
                }
                
                self.GetComponent<BBWait>().Notify(ops);
                self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgFtg>().Refresh(ops);
            }
        }

        [FriendOf(typeof (TODTimerComponent))]
        public class TODInputComponentAwakeSystem: AwakeSystem<BBInputComponent>
        {
            protected override void Awake(BBInputComponent self)
            {
                self.ClientScene().GetComponent<UIComponent>().ShowWindow<DlgFtg>();
                TODTimerComponent timerComponent = self.AddComponent<TODTimerComponent>();
                self.timer = timerComponent.NewFrameTimer(TODTimerInvokeType.CheckInput, self);
                self.AddComponent<BBWait>();
            }
        }

        [FriendOf(typeof (TODTimerComponent))]
        public class TODInputComponentLoadSystem: LoadSystem<BBInputComponent>
        {
            protected override void Load(BBInputComponent self)
            {
                self.ClientScene().GetComponent<UIComponent>().UnLoadWindow<DlgFtg>();
                self.ClientScene().GetComponent<UIComponent>().ShowWindow<DlgFtg>();

                self.RemoveComponent<TODTimerComponent>();
                TODTimerComponent timerComponent = self.AddComponent<TODTimerComponent>();
                self.timer = timerComponent.NewFrameTimer(TODTimerInvokeType.CheckInput, self);
            }
        }
    }
}