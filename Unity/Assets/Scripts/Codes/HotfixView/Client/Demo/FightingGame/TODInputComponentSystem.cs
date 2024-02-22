using UnityEngine.InputSystem;

namespace ET.Client
{
    public static class TODInputComponentSystem
    {
        [Invoke(TODTimerInvokeType.CheckInput)]
        [FriendOf(typeof (TODInputComponent))]
        [FriendOf(typeof (TODTimerComponent))]
        public class CheckInputTimer: TODTimer<TODInputComponent>
        {
            protected override void Run(TODInputComponent self)
            {
                long ops = FTGHelper.CheckInput();
                if (Gamepad.current.startButton.isPressed)
                {
                    EventSystem.Instance.Load();
                }

                //可能有大量帧的按键输入是连续且一致的，这里只存储相同帧的第一帧
                var curInfo = self.infos.Peek();
                curInfo.lastedFrame++;
                if (curInfo.ops != ops)
                {
                    self.infos.Enqueue(InputInfo.Create(self.GetComponent<TODTimerComponent>().curFrame, ops));
                    if (self.infos.Count > self.maxStackSize) self.infos.Dequeue().Recyle();
                }

                self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgFtg>().Refresh(ops);
            }
        }
        
        [FriendOf(typeof(TODTimerComponent))]
        public class TODInputComponentAwakeSystem : AwakeSystem<TODInputComponent>
        {
            protected override void Awake(TODInputComponent self)
            {
                self.ClientScene().GetComponent<UIComponent>().ShowWindow<DlgFtg>();
                TODTimerComponent timerComponent = self.AddComponent<TODTimerComponent>();
                self.timer = timerComponent.NewFrameTimer(TODTimerInvokeType.CheckInput, self);
                
                self.infos.Clear();
                self.infos.Enqueue(InputInfo.Create(self.GetComponent<TODTimerComponent>().curFrame, 0));
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
                
                self.infos.Clear();
                self.infos.Enqueue(InputInfo.Create(self.GetComponent<TODTimerComponent>().curFrame, 0));
            }
        }
    }
}