using UnityEngine;
using UnityEngine.InputSystem;

namespace ET.Client
{
    //开始重构
    [FriendOf(typeof (BBInputManager))]
    public static class BBInputManagerSystem
    {
        public class BBInputAwakeSystem: AwakeSystem<BBInputManager>
        {
            protected override void Awake(BBInputManager self)
            {
                BBInputManager.Instance = self;
            }
        }

        public static long CheckInput(this BBInputManager self)
        {
            //GamePad优先级高于Keyboard
            if (Gamepad.current != null)
            {
                return self.CheckInput_GamePad();
            }
            
            return self.CheckInput_Keyboard();
        }

        private static long CheckInput_Keyboard(this BBInputManager _)
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                Log.Error($"please insert keyboard!!!");
                return 0;
            }

            long ops = 0;

            // direction
            ops |= (uint)(keyboard.wKey.isPressed? BBOperaType.UP : 0);
            ops |= (uint)(keyboard.sKey.isPressed? BBOperaType.DOWN : 0);
            ops |= (uint)(keyboard.aKey.isPressed? BBOperaType.LEFT : 0);
            ops |= (uint)(keyboard.dKey.isPressed? BBOperaType.RIGHT : 0);
            ops |= (uint)(keyboard.wKey.isPressed && keyboard.dKey.isPressed? BBOperaType.UPRIGHT : 0);
            ops |= (uint)(keyboard.wKey.isPressed && keyboard.aKey.isPressed? BBOperaType.UPLEFT : 0);
            ops |= (uint)(keyboard.sKey.isPressed && keyboard.dKey.isPressed? BBOperaType.DOWNRIGHT : 0);
            ops |= (uint)(keyboard.sKey.isPressed && keyboard.aKey.isPressed? BBOperaType.DOWNLEFT : 0);
            ops |= (uint)(ops == 0 ? BBOperaType.MIDDLE : 0);
            
            // attack
            ops |= (uint)(keyboard.jKey.isPressed? BBOperaType.X : 0);
            ops |= (uint)(keyboard.kKey.isPressed? BBOperaType.Y : 0);
            ops |= (uint)(keyboard.lKey.isPressed? BBOperaType.A : 0);
            ops |= (uint)(keyboard.uKey.isPressed? BBOperaType.B : 0);
            ops |= (uint)(keyboard.iKey.isPressed? BBOperaType.RB : 0);
            ops |= (uint)(keyboard.oKey.isPressed? BBOperaType.RT : 0);
            ops |= (uint)(keyboard.nKey.isPressed? BBOperaType.LB : 0);
            ops |= (uint)(keyboard.mKey.isPressed? BBOperaType.LT : 0);
            
            return ops;
        }
        
        private static long CheckInput_GamePad(this BBInputManager _)
        {
             Gamepad gamepad = Gamepad.current;
            if (gamepad == null)
            {
                Log.Error($"please insert gamepad!!!");
                return 0;
            }
            
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
    }
}