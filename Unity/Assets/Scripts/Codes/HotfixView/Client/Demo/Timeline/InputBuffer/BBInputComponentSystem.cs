using UnityEngine;
using UnityEngine.InputSystem;

namespace ET.Client
{
    //开始重构
    [FriendOf(typeof (BBInputComponent))]
    public static class BBInputComponentSystem
    {
        public class BBInputAwakeSystem: AwakeSystem<BBInputComponent>
        {
            protected override void Awake(BBInputComponent self)
            {
                BBInputComponent.Instance = self;
            }
        }

        public static void Reload(this BBInputComponent self)
        {
            //TODO Init
            self.WasPressedDict.Clear();
            self.WasPressedDict.Add(BBOperaType.LIGHTPUNCH, false);
            self.WasPressedDict.Add(BBOperaType.LIGHTKICK, false);
            self.WasPressedDict.Add(BBOperaType.MIDDLEPUNCH, false);
            self.WasPressedDict.Add(BBOperaType.MIDDLEKICK, false);
            self.WasPressedDict.Add(BBOperaType.HEAVYPUNCH, false);
            self.WasPressedDict.Add(BBOperaType.HEAVYKICK, false);
        }

        public static void Update(this BBInputComponent self)
        {
            self.Ops = self.CheckInput();
            EventSystem.Instance.PublishAsync(self.DomainScene(), new UpdateInputCallback() { Ops = self.Ops }).Coroutine();
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

            // 轻拳
            if (gamepad.xButton.isPressed)
            {
                ops |= BBOperaType.LIGHTPUNCH;
            }

            //中拳
            if (gamepad.yButton.isPressed)
            {
                ops |= BBOperaType.MIDDLEPUNCH;
            }

            //重拳
            if (gamepad.rightShoulder.isPressed)
            {
                ops |= BBOperaType.HEAVYPUNCH;
            }

            //轻脚
            if (gamepad.aButton.isPressed)
            {
                ops |= BBOperaType.LIGHTKICK;
            }

            //中脚
            if (gamepad.bButton.isPressed)
            {
                ops |= BBOperaType.MIDDLEKICK;
            }

            //重脚
            if (gamepad.rightTrigger.isPressed)
            {
                ops |= BBOperaType.HEAVYKICK;
            }
            
            
            //LB（组合键）
            if (gamepad.leftShoulder.isPressed)
            {
                ops |= BBOperaType.HEAVYKICK | BBOperaType.HEAVYPUNCH;
            }

            //LT
            if (gamepad.leftTrigger.isPressed)
            {
                ops |= BBOperaType.MIDDLEKICK | BBOperaType.MIDDLEPUNCH;
            }

            return ops;
        }

        public static bool ContainKey(this BBInputComponent self, long op)
        {
            return (self.Ops & op) != 0;
        }

        public static bool WasPressedThisFrame(this BBInputComponent self, int op)
        {
            return self.WasPressedDict[op];
        }
    }
}