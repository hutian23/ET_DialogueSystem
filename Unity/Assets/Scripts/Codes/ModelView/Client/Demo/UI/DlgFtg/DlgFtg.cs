using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (UIBaseWindow))]
    public class DlgFtg: Entity, IAwake, IUILogic, ILoad
    {
        public DlgFtgViewComponent View
        {
            get => this.GetComponent<DlgFtgViewComponent>();
        }

        public Dictionary<int, Scroll_Item_OPInfo> opInfos = new();

        public long currentOP;
        public Queue<OpInfo> OPQueue = new();
        public int maxStack = 18;

        //只加载一次
        public Sprite arrow_Up;
        public Sprite arrow_UpRight;
        public Sprite arrow_Right;
        public Sprite arrow_DownRight;
        public Sprite arrow_Down;
        public Sprite arrow_DownLeft;
        public Sprite arrow_Left;
        public Sprite arrow_UpLeft;
        public Sprite arrow_None;

        public Sprite lp;
        public Sprite lk;
        public Sprite mp;
        public Sprite mk;
        public Sprite hk;
        public Sprite hp;
    }

    public struct OpInfo
    {
        //当前指令持续了多少帧,最大显示99
        public int LastedFrame;
        public long OP;
    }

    public static class FrameDataType
    {
        public const string None = "#413F3F96";
    }
}