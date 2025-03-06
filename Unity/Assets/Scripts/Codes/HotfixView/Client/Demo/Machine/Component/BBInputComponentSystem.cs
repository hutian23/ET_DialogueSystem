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
            self.WasPressedDict.Clear();
            self.WasPressedDict.Add(BBOperaType.X, false);
            self.WasPressedDict.Add(BBOperaType.A, false);
            self.WasPressedDict.Add(BBOperaType.Y, false);
            self.WasPressedDict.Add(BBOperaType.B, false);
            self.WasPressedDict.Add(BBOperaType.RB, false);
            self.WasPressedDict.Add(BBOperaType.RT, false);
        }

        public static long CheckInput(this BBInputComponent self)
        {
            Gamepad gamepad = Gamepad.current;
            long ops = 0;

            //1. 方向键
            Vector2 direction = gamepad.leftStick.ReadValue();
            
            // Log.Warning(gamepad.leftStickButton.isPressed.ToString());
            
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
                ops |= BBOperaType.X;
            }

            //中拳
            if (gamepad.yButton.isPressed)
            {
                ops |= BBOperaType.Y;
            }

            //重拳
            if (gamepad.rightShoulder.isPressed)
            {
                ops |= BBOperaType.RB;
            }

            //轻脚
            if (gamepad.aButton.isPressed)
            {
                ops |= BBOperaType.A;
            }

            //中脚
            if (gamepad.bButton.isPressed)
            {
                ops |= BBOperaType.B;
            }

            //重脚
            if (gamepad.rightTrigger.isPressed)
            {
                ops |= BBOperaType.RT;
            }
            
            //LB（组合键）
            if (gamepad.leftShoulder.isPressed)
            {
                ops |= BBOperaType.LB;
            }

            //LT
            if (gamepad.leftTrigger.isPressed)
            {
                ops |= BBOperaType.LT;
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