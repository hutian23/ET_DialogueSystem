using UnityEngine.InputSystem;

namespace ET.Client
{
    public static class TODInputComponentSystem
    {
        [Invoke(TODTimerInvokeType.CheckInput)]
        [FriendOf(typeof (TODInputComponent))]
        public class CheckInputTimer: TODTimer<TODInputComponent>
        {
            protected override void Run(TODInputComponent self)
            {
                long ops = FTGHelper.CheckInput();
                if (Gamepad.current.startButton.isPressed)
                {
                    EventSystem.Instance.Load();
                }

                self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgFtg>().Refresh(ops);
            }
        }

        public class TODInputComponentAwakeSystem: AwakeSystem<TODInputComponent>
        {
            protected override void Awake(TODInputComponent self)
            {
                self.ClientScene().GetComponent<UIComponent>().ShowWindow<DlgFtg>();
                TODTimerComponent timerComponent = self.AddComponent<TODTimerComponent>();
                self.timer = timerComponent.NewFrameTimer(TODTimerInvokeType.CheckInput, self);
            }
        }

        [FriendOf(typeof (TODTimerComponent))]
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
    }
}