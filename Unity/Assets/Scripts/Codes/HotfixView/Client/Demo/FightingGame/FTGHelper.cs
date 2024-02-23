using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ET.Client
{
    [FriendOf(typeof(BBInputComponent))]
    [FriendOf(typeof(TODTimerComponent))]
    public static class FTGHelper
    {
        public static long CheckInput()
        {
            Gamepad gamepad = Gamepad.current;
            long ops = 0;

            //1. 方向键
            Vector2 direction = gamepad.leftStick.ReadValue();
            if (direction.magnitude <= 0.55f) //手柄漂移问题
            {
                ops |= TODOperaType.MIDDLE;
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
                    ops |= TODOperaType.UPRIGHT;
                }
                else if (angle is >= 67.5f and < 112.5f)
                {
                    ops |= TODOperaType.UP;
                }
                else if (angle is >= 112.5f and < 157.5f)
                {
                    ops |= TODOperaType.UPLEFT;
                }
                else if (angle is >= 157.5f and < 202.5f)
                {
                    ops |= TODOperaType.LEFT;
                }
                else if (angle is >= 202.5f and < 247.5f)
                {
                    ops |= TODOperaType.DOWNLEFT;
                }
                else if (angle is >= 247.5f and < 292.5f)
                {
                    ops |= TODOperaType.DOWN;
                }
                else if (angle is >= 292.5f and < 337.5f)
                {
                    ops |= TODOperaType.DOWNRIGHT;
                }
                else
                {
                    ops |= TODOperaType.RIGHT;
                }
            }

            //2. 技能按键
            if (gamepad.buttonSouth.isPressed)
            {
                ops |= TODOperaType.LIGHTKICK;
            }

            if (gamepad.buttonEast.isPressed)
            {
                ops |= TODOperaType.MIDDLEKICK;
            }

            if (gamepad.buttonNorth.isPressed)
            {
                ops |= TODOperaType.MIDDLEPUNCH;
            }

            if (gamepad.buttonWest.isPressed)
            {
                ops |= TODOperaType.LIGHTPUNCH;
            }

            // 检测 RB 按钮
            if (gamepad.rightShoulder.isPressed)
            {
                ops |= TODOperaType.HEAVYPUNCH;
            }

            // 检测 LB 按钮
            if (gamepad.leftShoulder.isPressed)
            {
                ops |= TODOperaType.HEAVYPUNCH;
                ops |= TODOperaType.HEAVYKICK;
            }

            // LT
            if (gamepad.leftTrigger.isPressed)
            {
                ops |= TODOperaType.MIDDLEPUNCH;
                ops |= TODOperaType.MIDDLEKICK;
            }

            // RT
            if (gamepad.rightTrigger.isPressed)
            {
                ops |= TODOperaType.HEAVYKICK;
            }

            return ops;
        }

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

        //获得按键输入缓冲区
        public static Queue<InputInfo> GetBuffer(Unit unit)
        {
            return unit.GetComponent<DialogueComponent>().GetComponent<BBInputComponent>().infos;
        }

        //获取输入缓冲组件当前帧号
        public static long GetCurFrame(Unit unit)
        {
            return unit.GetComponent<DialogueComponent>().GetComponent<BBInputComponent>().GetComponent<TODTimerComponent>().curFrame;
        }
    }
}