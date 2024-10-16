﻿using UnityEngine;

namespace ET
{
    public static class Constants
    {
        public const float STEP = 0.1f;
        public const float DEVIATION = 0.02f;

        public static LayerMask GroundMask = LayerMask.NameToLayer("Ground");
        public static LayerMask WallMask;

        public static int ClimbCheckDist = 2; // 攀爬检查的像素值
        public static int ClimbUpCheckDist = 2; // 向上检测墙壁的像素值
        public static float ClimbGrabYMult = .2f;

        public static int DashCornerCorrection = 4;
        public static int UpwardCornerCorrection = 4;

        public static int Enemy_InvincibleTime = 100;

        public const float MaxFall = -40f;
        public const float Gravity = 90f;

        #region 打字
        public static readonly string[] _uguiSymbols = { "b", "i" };
        public static readonly string[] _uguiCloseSymbols = { "b", "i", "size", "color" };
        //默认打字速度是间隔200ms打印一次
        public const int TypeSpeed = 100;
        #endregion
    }
}