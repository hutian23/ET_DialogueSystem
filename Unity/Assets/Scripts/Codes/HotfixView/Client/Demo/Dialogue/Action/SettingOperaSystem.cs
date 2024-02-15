using UnityEngine.InputSystem;

namespace ET.Client
{
    // 实现update多态
    [FriendOf(typeof (SettingOpera))]
    public static class SettingOperaSystem
    {
        [Invoke(TimerInvokeType.SettingTimer)]
        public class SettingOperaTimer: ATimer<SettingOpera>
        {
            protected override void Run(SettingOpera self)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    self.EnableSettingCheck();
                    self.ClientScene().GetComponent<UIComponent>().HideWindow<DlgStorage>();
                }
            }
        }

        [Invoke(TimerInvokeType.SettingCheckTimer)]
        public class SettingOperaCheckTimer: ATimer<SettingOpera>
        {
            protected override void Run(SettingOpera self)
            {
                DialogueComponent dialogueComponent = self.GetParent<DialogueComponent>();
                if (!dialogueComponent.ContainTag(DialogueTag.CanEnterSetting)) return;
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    self.EnabelSettingView();
                }
            }
        }
        
        public class SettingOperaDestroySystem: DestroySystem<SettingOpera>
        {
            protected override void Destroy(SettingOpera self)
            {
                self.RemoveTimer();
                self.ClientScene().GetComponent<UIComponent>().CloseWindow<DlgStorage>();
            }
        }

        private static void RemoveTimer(this SettingOpera self)
        {
            TimerComponent.Instance.Remove(ref self.controllerTimer);
        }

        public static void EnableSettingCheck(this SettingOpera self)
        {
            self.RemoveTimer();
            //注意是下一帧执行
            self.controllerTimer = TimerComponent.Instance.NewFrameTimer(TimerInvokeType.SettingCheckTimer, self);
        }

        private static void EnabelSettingView(this SettingOpera self)
        {
            self.RemoveTimer();
            self.controllerTimer = TimerComponent.Instance.NewFrameTimer(TimerInvokeType.SettingTimer, self);
            
            //刷新存档界面
            self.ClientScene().GetComponent<UIComponent>().ShowWindow<DlgStorage>();
            DlgStorage dlgStorage = self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgStorage>();
            
        }
    }
}