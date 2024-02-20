using System.Collections.Generic;

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
    }

    public struct OpInfo
    {
        //当前指令持续了多少帧,最大显示99
        public int LastedFrame;
        public long OP;
    }
}