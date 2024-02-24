using System.Collections.Generic;
namespace ET.Client
{
    [FriendOf(typeof(BBInputComponent))]
    [FriendOf(typeof(TODTimerComponent))]
    public static class FTGHelper
    {
        // public static long CheckInput()
        // {
        //     Gamepad gamepad = Gamepad.current;
        //     long ops = 0;
        //
        //     //1. 方向键
        //     Vector2 direction = gamepad.leftStick.ReadValue();
        //     if (direction.magnitude <= 0.55f) //手柄漂移问题
        //     {
        //         ops |= BBOperaType.MIDDLE;
        //     }
        //     else
        //     {
        //         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //         if (angle < 0)
        //         {
        //             angle += 360f;
        //         }
        //
        //         if (angle is >= 22.5f and < 67.5f)
        //         {
        //             ops |= BBOperaType.UPRIGHT;
        //         }
        //         else if (angle is >= 67.5f and < 112.5f)
        //         {
        //             ops |= BBOperaType.UP;
        //         }
        //         else if (angle is >= 112.5f and < 157.5f)
        //         {
        //             ops |= BBOperaType.UPLEFT;
        //         }
        //         else if (angle is >= 157.5f and < 202.5f)
        //         {
        //             ops |= BBOperaType.LEFT;
        //         }
        //         else if (angle is >= 202.5f and < 247.5f)
        //         {
        //             ops |= BBOperaType.DOWNLEFT;
        //         }
        //         else if (angle is >= 247.5f and < 292.5f)
        //         {
        //             ops |= BBOperaType.DOWN;
        //         }
        //         else if (angle is >= 292.5f and < 337.5f)
        //         {
        //             ops |= BBOperaType.DOWNRIGHT;
        //         }
        //         else
        //         {
        //             ops |= BBOperaType.RIGHT;
        //         }
        //     }
        //
        //     //2. 技能按键
        //     if (gamepad.buttonSouth.isPressed)
        //     {
        //         ops |= BBOperaType.LIGHTKICK;
        //     }
        //
        //     if (gamepad.buttonEast.wasPressedThisFrame)
        //     {
        //         ops |= BBOperaType.MIDDLEKICK;
        //     }
        //
        //     if (gamepad.buttonNorth.wasPressedThisFrame)
        //     {
        //         ops |= BBOperaType.MIDDLEPUNCH;
        //     }
        //
        //     if (gamepad.buttonWest.wasPressedThisFrame)
        //     {
        //         ops |= BBOperaType.LIGHTPUNCH;
        //     }
        //
        //     // 检测 RB 按钮
        //     if (gamepad.rightShoulder.wasPressedThisFrame)
        //     {
        //         ops |= BBOperaType.HEAVYPUNCH;
        //     }
        //
        //     // 检测 LB 按钮
        //     if (gamepad.leftShoulder.wasPressedThisFrame)
        //     {
        //         ops |= BBOperaType.HEAVYPUNCH;
        //         ops |= BBOperaType.HEAVYKICK;
        //     }
        //
        //     // LT
        //     if (gamepad.leftTrigger.wasPressedThisFrame)
        //     {
        //         ops |= BBOperaType.MIDDLEPUNCH;
        //         ops |= BBOperaType.MIDDLEKICK;
        //     }
        //
        //     // RT
        //     if (gamepad.rightTrigger.wasPressedThisFrame)
        //     {
        //         ops |= BBOperaType.HEAVYKICK;
        //     }
        //
        //     return ops;
        // }

        public static List<int> GetInput(long ops)
        {
            var tmpList = new List<int>();
            for (int i = 0; i < 64; i++)
            {
                if ((ops & (2 << i)) != 0)
                {
                    tmpList.Add(i);
                }
            }

            return tmpList;
        }

        //获取输入缓冲组件当前帧号
        public static long GetCurFrame(Unit unit)
        {
            return unit.GetComponent<DialogueComponent>().GetComponent<BBInputComponent>().GetComponent<TODTimerComponent>().curFrame;
        }

        public static BBWait GetBBWait(Unit unit)
        {
            return unit.GetComponent<DialogueComponent>().GetComponent<BBInputComponent>().GetComponent<BBWait>();
        }
    }
}