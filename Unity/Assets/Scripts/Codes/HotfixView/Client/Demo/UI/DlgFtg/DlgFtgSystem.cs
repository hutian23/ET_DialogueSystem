using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace ET.Client
{
    [FriendOf(typeof (DlgFtg))]
    [FriendOf(typeof (Scroll_Item_OPInfo))]
    public static class DlgFtgSystem
    {
        public static void RegisterUIEvent(this DlgFtg self)
        {
            //清空滑动列表
            self.InitLoopScroll();
            self.AddUIScrollItems(ref self.opInfos, 18);
            self.View.ELoopScrollList_InputLoopVerticalScrollRect.AddItemRefreshListener(self.LoopRefresh);
            self.View.ELoopScrollList_InputLoopVerticalScrollRect.SetVisible(true, 18);
            //加载Icon
            self.arrow_Down = IconHelper.LoadIconSprite("OPInfo", "Arrow_Down");
            self.arrow_DownRight = IconHelper.LoadIconSprite("OPInfo", "Arrow_DownRight");
            self.arrow_Right = IconHelper.LoadIconSprite("OPInfo", "Arrow_Right");
            self.arrow_UpRight = IconHelper.LoadIconSprite("OPInfo", "Arrow_UpRight");
            self.arrow_Up = IconHelper.LoadIconSprite("OPInfo", "Arrow_Up");
            self.arrow_UpLeft = IconHelper.LoadIconSprite("OPInfo", "Arrow_UpLeft");
            self.arrow_Left = IconHelper.LoadIconSprite("OPInfo", "Arrow_Left");
            self.arrow_DownLeft = IconHelper.LoadIconSprite("OPInfo", "Arrow_DownLeft");
            self.arrow_None = IconHelper.LoadIconSprite("OPInfo", "Arrow_None");
            self.lp = IconHelper.LoadIconSprite("OPInfo", "OP_LP");
            self.lk = IconHelper.LoadIconSprite("OPInfo", "OP_LK");
            self.mp = IconHelper.LoadIconSprite("OPInfo", "OP_MP");
            self.mk = IconHelper.LoadIconSprite("OPInfo", "OP_MK");
            self.hp = IconHelper.LoadIconSprite("OPInfo", "OP_HP");
            self.hk = IconHelper.LoadIconSprite("OPInfo", "OP_HK");
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
            self.opInfos[index].uiTransform.Find("OPs").SetVisible(false);
        }

        public static void RefreshUI(this DlgFtg self, string text)
        {
            self.View.E_SkillText.SetText(text);
        }

        public static void Refresh(this DlgFtg self, long ops)
        {
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

            //本帧内无更新指令
            if (self.currentOP == tmp) return;
            count = self.OPQueue.Count;
            while (count-- > 0)
            {
                OpInfo opInfo = self.OPQueue.Dequeue();
                Transform parent = self.opInfos[count].uiTransform.Find("OPs");
                parent.SetVisible(true);
                ReferenceCollector refer = parent.GetComponent<ReferenceCollector>();
                //1. 方向键
                if ((opInfo.OP & BBOperaType.DOWN) != 0)
                {
                    refer.Get<GameObject>("Direction").GetComponent<Image>().overrideSprite = self.arrow_Down;
                }
                else if ((opInfo.OP & BBOperaType.DOWNRIGHT) != 0)
                {
                    refer.Get<GameObject>("Direction").GetComponent<Image>().overrideSprite = self.arrow_DownRight;
                }
                else if ((opInfo.OP & BBOperaType.RIGHT) != 0)
                {
                    refer.Get<GameObject>("Direction").GetComponent<Image>().overrideSprite = self.arrow_Right;
                }
                else if ((opInfo.OP & BBOperaType.UPRIGHT) != 0)
                {
                    refer.Get<GameObject>("Direction").GetComponent<Image>().overrideSprite = self.arrow_UpRight;
                }
                else if ((opInfo.OP & BBOperaType.UP) != 0)
                {
                    refer.Get<GameObject>("Direction").GetComponent<Image>().overrideSprite = self.arrow_Up;
                }
                else if ((opInfo.OP & BBOperaType.UPLEFT) != 0)
                {
                    refer.Get<GameObject>("Direction").GetComponent<Image>().overrideSprite = self.arrow_UpLeft;
                }
                else if ((opInfo.OP & BBOperaType.LEFT) != 0)
                {
                    refer.Get<GameObject>("Direction").GetComponent<Image>().overrideSprite = self.arrow_Left;
                }
                else if ((opInfo.OP & BBOperaType.DOWNLEFT) != 0)
                {
                    refer.Get<GameObject>("Direction").GetComponent<Image>().overrideSprite = self.arrow_DownLeft;
                }
                else
                {
                    refer.Get<GameObject>("Direction").GetComponent<Image>().overrideSprite = self.arrow_None;
                }

                long op = opInfo.OP;
                for (int i = 0; i < 6; i++)
                {
                    Image op_UI = refer.Get<GameObject>($"OP{i + 1}").GetComponent<Image>();
                    op_UI.SetVisible(true);
                    if ((op & BBOperaType.LIGHTPUNCH) != 0)
                    {
                        op_UI.overrideSprite = self.lp;
                        op ^= BBOperaType.LIGHTPUNCH;
                    }
                    else if ((op & BBOperaType.LIGHTKICK) != 0)
                    {
                        op_UI.overrideSprite = self.lk;
                        op ^= BBOperaType.LIGHTKICK;
                    }
                    else if ((op & BBOperaType.MIDDLEPUNCH) != 0)
                    {
                        op_UI.overrideSprite = self.mp;
                        op ^= BBOperaType.MIDDLEPUNCH;
                    }
                    else if ((op & BBOperaType.MIDDLEKICK) != 0)
                    {
                        op_UI.overrideSprite = self.mk;
                        op ^= BBOperaType.MIDDLEKICK;
                    }
                    else if ((op & BBOperaType.HEAVYPUNCH) != 0)
                    {
                        op_UI.overrideSprite = self.hp;
                        op ^= BBOperaType.HEAVYPUNCH;
                    }
                    else if ((op & BBOperaType.HEAVYKICK) != 0)
                    {
                        op_UI.overrideSprite = self.hk;
                        op ^= BBOperaType.HEAVYKICK;
                    }
                    else
                    {
                        op_UI.SetVisible(false);
                    }
                }

                self.OPQueue.Enqueue(opInfo);
            }

            //每帧刷新控制器UI
            float disable = 0.3f, enable = 1;
            self.View.E_Arrow_DownImage.Setalpha((ops & BBOperaType.DOWN) != 0? enable : disable);
            self.View.E_Arrow_DownRightImage.Setalpha((ops & BBOperaType.DOWNRIGHT) != 0? enable : disable);
            self.View.E_Arrow_RightImage.Setalpha((ops & BBOperaType.RIGHT) != 0? enable : disable);
            self.View.E_Arrow_UpRightImage.Setalpha((ops & BBOperaType.UPRIGHT) != 0? enable : disable);
            self.View.E_Arrow_UpImage.Setalpha((ops & BBOperaType.UP) != 0? enable : disable);
            self.View.E_Arrow_UpLeftImage.Setalpha((ops & BBOperaType.UPLEFT) != 0? enable : disable);
            self.View.E_Arrow_LeftImage.Setalpha((ops & BBOperaType.LEFT) != 0? enable : disable);
            self.View.E_Arrow_DownLeftImage.Setalpha((ops & BBOperaType.DOWNLEFT) != 0? enable : disable);

            self.View.E_LightPunchImage.Setalpha((ops & BBOperaType.LIGHTPUNCH) != 0? enable : disable);
            self.View.E_LightKickImage.Setalpha((ops & BBOperaType.LIGHTKICK) != 0? enable : disable);
            self.View.E_MiddlePunchImage.Setalpha((ops & BBOperaType.MIDDLEPUNCH) != 0? enable : disable);
            self.View.E_MiddleKickImage.Setalpha((ops & BBOperaType.MIDDLEKICK) != 0? enable : disable);
            self.View.E_HeavyPunchImage.Setalpha((ops & BBOperaType.HEAVYPUNCH) != 0? enable : disable);
            self.View.E_HeavyKickImage.Setalpha((ops & BBOperaType.HEAVYKICK) != 0? enable : disable);
        }
    }
}