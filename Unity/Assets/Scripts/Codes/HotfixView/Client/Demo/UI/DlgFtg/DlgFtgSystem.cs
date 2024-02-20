using MongoDB.Bson;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (DlgFtg))]
    public static class DlgFtgSystem
    {
        public static void RegisterUIEvent(this DlgFtg self)
        {
            //清空滑动列表
            self.InitLoopScroll();
            self.AddUIScrollItems(ref self.opInfos, 18);
            self.View.ELoopScrollList_InputLoopVerticalScrollRect.AddItemRefreshListener(self.LoopRefresh);
            self.View.ELoopScrollList_InputLoopVerticalScrollRect.SetVisible(true, 18);
        }

        public static void ShowWindow(this DlgFtg self, Entity contextData = null)
        {
        }

        public static void InitLoopScroll(this DlgFtg self)
        {
            self.View.ELoopScrollList_InputLoopVerticalScrollRect.SetVisible(false);
        }

        private static void LoopRefresh(this DlgFtg self, Transform transform, int index)
        {
            self.opInfos[index].BindTrans(transform);
            self.opInfos[index].E_FrameText.text = "";
        }

        public static void Refresh(this DlgFtg self, long ops)
        {
            //每帧刷新控制器UI
            float disable = 0.3f, enable = 1;
            self.View.E_Arrow_DownImage.Setalpha((ops & TODOperaType.DOWN) != 0? enable : disable);
            self.View.E_Arrow_DownRightImage.Setalpha((ops & TODOperaType.DOWNRIGHT) != 0? enable : disable);
            self.View.E_Arrow_RightImage.Setalpha((ops & TODOperaType.RIGHT) != 0? enable : disable);
            self.View.E_Arrow_UpRightImage.Setalpha((ops & TODOperaType.UPRIGHT) != 0? enable : disable);
            self.View.E_Arrow_UpImage.Setalpha((ops & TODOperaType.UP) != 0? enable : disable);
            self.View.E_Arrow_UpLeftImage.Setalpha((ops & TODOperaType.UPLEFT) != 0? enable : disable);
            self.View.E_Arrow_LeftImage.Setalpha((ops & TODOperaType.LEFT) != 0? enable : disable);
            self.View.E_Arrow_DownLeftImage.Setalpha((ops & TODOperaType.DOWNLEFT) != 0? enable : disable);

            self.View.E_LightPunchImage.Setalpha((ops & TODOperaType.LIGHTPUNCH) != 0? enable : disable);
            self.View.E_LightKickImage.Setalpha((ops & TODOperaType.LIGHTKICK) != 0? enable : disable);
            self.View.E_MiddlePunchImage.Setalpha((ops & TODOperaType.MIDDLEPUNCH) != 0? enable : disable);
            self.View.E_MiddleKickImage.Setalpha((ops & TODOperaType.MIDDLEKICK) != 0? enable : disable);
            self.View.E_HeavyPunchImage.Setalpha((ops & TODOperaType.HEAVYPUNCH) != 0? enable : disable);
            self.View.E_HeavyKickImage.Setalpha((ops & TODOperaType.HEAVYKICK) != 0? enable : disable);

            //更新指令历史队列
            //当前帧指令改变了
            long tmp = self.currentOP;
            if (self.currentOP != ops)
            {
                //超出队列最大长度
                self.OPQueue.Enqueue(new OpInfo() { OP = ops });
                if (self.OPQueue.Count > self.maxStack) self.OPQueue.Dequeue();
                self.currentOP = ops;
            }

            int count = self.OPQueue.Count;
            while (count-- > 0)
            {
                OpInfo opInfo = self.OPQueue.Dequeue();
                if (count == 0 && opInfo.LastedFrame < 99) opInfo.LastedFrame++; //当前指令的持续时间
                self.opInfos[count].E_FrameText.SetText(opInfo.LastedFrame.ToString());
                self.OPQueue.Enqueue(opInfo);
            }

            //当前指令无改变，不刷新
            if (self.currentOP == tmp) return;
            count = self.OPQueue.Count;
            while (count-- > 0)
            {
                OpInfo opInfo = self.OPQueue.Dequeue();
                long op = opInfo.OP;
                Log.Warning(FTGHelper.GetInput(op).ToJson());
                self.OPQueue.Enqueue(opInfo);
            }
        }
    }
}