using UnityEngine;
using UnityEngine.InputSystem;

namespace ET.Client
{
    [FriendOf(typeof(BBInputComponent))]
    public static class TODInputComponentSystem
    {
        [Invoke(BBTimerInvokeType.CheckInput)]
        [FriendOf(typeof(BBInputComponent))]
        [FriendOf(typeof(BBTimerComponent))]
        public class CheckInputTimer : BBTimer<BBInputComponent>
        {
            protected override void Run(BBInputComponent self)
            {
                long ops = self.CheckInput();
                self.GetComponent<BBWait>().Notify(ops);
                self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgFtg>().Refresh(ops);
            }
        }

        [FriendOf(typeof(BBTimerComponent))]
        public class TODInputComponentAwakeSystem : AwakeSystem<BBInputComponent>
        {
            protected override void Awake(BBInputComponent self)
            {
                self.ClientScene().GetComponent<UIComponent>().ShowWindow<DlgFtg>();

                BBTimerComponent timerComponent = self.AddComponent<BBTimerComponent>();
                self.timer = timerComponent.NewFrameTimer(BBTimerInvokeType.CheckInput, self);

                self.AddComponent<BBWait>();
                self.InitPressDict();
            }
        }

        [FriendOf(typeof(BBTimerComponent))]
        public class TODInputComponentLoadSystem : LoadSystem<BBInputComponent>
        {
            protected override void Load(BBInputComponent self)
            {
                self.ClientScene().GetComponent<UIComponent>().UnLoadWindow<DlgFtg>();
                self.ClientScene().GetComponent<UIComponent>().ShowWindow<DlgFtg>();

                self.RemoveComponent<BBTimerComponent>();
                BBTimerComponent timerComponent = self.AddComponent<BBTimerComponent>();
                self.timer = timerComponent.NewFrameTimer(BBTimerInvokeType.CheckInput, self);

                self.InitPressDict();
            }
        }

        private static void InitPressDict(this BBInputComponent self)
        {
            self.pressDict.Clear();
            self.pressDict.Add(BBOperaType.LIGHTPUNCH, 0);
            self.pressDict.Add(BBOperaType.MIDDLEPUNCH, 0);
            self.pressDict.Add(BBOperaType.HEAVYPUNCH, 0);

            self.pressDict.Add(BBOperaType.LIGHTKICK, 0);
            self.pressDict.Add(BBOperaType.MIDDLEKICK, 0);
            self.pressDict.Add(BBOperaType.HEAVYKICK, 0);

            self.pressDict.Add(BBOperaType.HEAVYPUNCH | BBOperaType.HEAVYKICK, 0);
            self.pressDict.Add(BBOperaType.MIDDLEKICK | BBOperaType.MIDDLEPUNCH, 0);
        }

        public static long CheckInput(this BBInputComponent self)
        {
            Gamepad gamepad = Gamepad.current;
            long ops = 0;

            //1. 方向键
            Vector2 direction = gamepad.leftStick.ReadValue();
            if (direction.magnitude <= 0.45f) //手柄漂移问题
            {
                ops |= BBOperaType.MIDDLE;
            }
            else
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (angle < 0)
                {
                    angle += 360f;
                }

                if (angle is >= 22.5f and < 67.5f)
                {
                    ops |= BBOperaType.UPRIGHT;
                }
                else if (angle is >= 67.5f and < 112.5f)
                {
                    ops |= BBOperaType.UP;
                }
                else if (angle is >= 112.5f and < 157.5f)
                {
                    ops |= BBOperaType.UPLEFT;
                }
                else if (angle is >= 157.5f and < 202.5f)
                {
                    ops |= BBOperaType.LEFT;
                }
                else if (angle is >= 202.5f and < 247.5f)
                {
                    ops |= BBOperaType.DOWNLEFT;
                }
                else if (angle is >= 247.5f and < 292.5f)
                {
                    ops |= BBOperaType.DOWN;
                }
                else if (angle is >= 292.5f and < 337.5f)
                {
                    ops |= BBOperaType.DOWNRIGHT;
                }
                else
                {
                    ops |= BBOperaType.RIGHT;
                }
            }

            //2. 技能按键
            BBTimerComponent timerComponent = self.GetComponent<BBTimerComponent>();
            // 轻拳
            if (gamepad.xButton.isPressed)
            {
                if (self.pressDict[BBOperaType.LIGHTPUNCH] == 0) self.pressDict[BBOperaType.LIGHTPUNCH] = timerComponent.GetNow();
                if (timerComponent.GetNow() - self.pressDict[BBOperaType.LIGHTPUNCH] < 10) ops |= BBOperaType.LIGHTPUNCH;
            }
            else
            {
                self.pressDict[BBOperaType.LIGHTPUNCH] = 0;
            }

            //中拳
            if (gamepad.yButton.isPressed)
            {
                if (self.pressDict[BBOperaType.MIDDLEPUNCH] == 0) self.pressDict[BBOperaType.MIDDLEPUNCH] = timerComponent.GetNow();
                if (timerComponent.GetNow() - self.pressDict[BBOperaType.MIDDLEPUNCH] < self.maxPressedFrame) ops |= BBOperaType.MIDDLEPUNCH;
            }
            else
            {
                self.pressDict[BBOperaType.MIDDLEPUNCH] = 0;
            }

            //重拳
            if (gamepad.rightShoulder.isPressed)
            {
                if (self.pressDict[BBOperaType.HEAVYPUNCH] == 0) self.pressDict[BBOperaType.HEAVYPUNCH] = timerComponent.GetNow();
                if (timerComponent.GetNow() - self.pressDict[BBOperaType.HEAVYPUNCH] < self.maxPressedFrame) ops |= BBOperaType.HEAVYPUNCH;
            }
            else
            {
                self.pressDict[BBOperaType.HEAVYPUNCH] = 0;
            }

            //轻脚
            if (gamepad.aButton.isPressed)
            {
                if (self.pressDict[BBOperaType.LIGHTKICK] == 0) self.pressDict[BBOperaType.LIGHTKICK] = timerComponent.GetNow();
                if (timerComponent.GetNow() - self.pressDict[BBOperaType.LIGHTKICK] < self.maxPressedFrame) ops |= BBOperaType.LIGHTKICK;
            }
            else
            {
                self.pressDict[BBOperaType.LIGHTKICK] = 0;
            }

            //中脚
            if (gamepad.bButton.isPressed)
            {
                if (self.pressDict[BBOperaType.MIDDLEKICK] == 0) self.pressDict[BBOperaType.MIDDLEKICK] = timerComponent.GetNow();
                if (timerComponent.GetNow() - self.pressDict[BBOperaType.MIDDLEKICK] < self.maxPressedFrame) ops |= BBOperaType.MIDDLEKICK;
            }
            else
            {
                self.pressDict[BBOperaType.MIDDLEKICK] = 0;
            }

            //重脚
            if (gamepad.rightTrigger.isPressed)
            {
                if (self.pressDict[BBOperaType.HEAVYKICK] == 0) self.pressDict[BBOperaType.HEAVYKICK] = timerComponent.GetNow();
                if (timerComponent.GetNow() - self.pressDict[BBOperaType.HEAVYKICK] < self.maxPressedFrame) ops |= BBOperaType.HEAVYKICK;
            }
            else
            {
                self.pressDict[BBOperaType.HEAVYKICK] = 0;
            }

            //LB（组合键）
            if (gamepad.leftShoulder.isPressed)
            {
                if (self.pressDict[BBOperaType.HEAVYPUNCH | BBOperaType.HEAVYKICK] == 0)
                    self.pressDict[BBOperaType.HEAVYPUNCH | BBOperaType.HEAVYKICK] = timerComponent.GetNow();
                if (timerComponent.GetNow() - self.pressDict[BBOperaType.HEAVYPUNCH | BBOperaType.HEAVYKICK] < self.maxPressedFrame)
                    ops |= BBOperaType.HEAVYKICK | BBOperaType.HEAVYPUNCH;
            }
            else
            {
                self.pressDict[BBOperaType.HEAVYPUNCH | BBOperaType.HEAVYKICK] = 0;
            }

            //LT
            if (gamepad.leftTrigger.isPressed)
            {
                if (self.pressDict[BBOperaType.MIDDLEPUNCH | BBOperaType.MIDDLEKICK] == 0)
                    self.pressDict[BBOperaType.MIDDLEPUNCH | BBOperaType.MIDDLEKICK] = timerComponent.GetNow();
                if (timerComponent.GetNow() - self.pressDict[BBOperaType.MIDDLEPUNCH | BBOperaType.MIDDLEKICK] < self.maxPressedFrame)
                    ops |= BBOperaType.MIDDLEKICK | BBOperaType.MIDDLEPUNCH;
            }
            else
            {
                self.pressDict[BBOperaType.MIDDLEKICK | BBOperaType.MIDDLEPUNCH] = 0;
            }

            return ops;
        }
    }
}