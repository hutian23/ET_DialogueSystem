namespace ET.Client
{
    [FriendOf(typeof (CancelManager))]
    public static class CancelManagerSystem
    {
        [Invoke(BBTimerInvokeType.CancelWindowTimer)]
        [FriendOf(typeof (CancelManager))]
        [FriendOf(typeof (SkillInfo))]
        public class CancelWindowTimer: BBTimer<CancelManager>
        {
            protected override void Run(CancelManager self)
            {
                SkillInfo curInfo = Root.Instance.Get(self.infoId) as SkillInfo;
                SkillBuffer buffer = self.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();

                //遍历SkillInfo
                foreach (Entity child in buffer.Children.Values)
                {
                    SkillInfo info = child as SkillInfo;
                    //层级更高，可取消
                    if (curInfo.moveType < info.moveType)
                    {
                        self.CancelableDict[info.behaviorOrder] = true;
                    }
                    //Move 同层可相互切换
                    else if (curInfo.moveType is MoveType.Move && info.moveType is MoveType.Move)
                    {
                        self.CancelableDict[info.behaviorOrder] = true;
                    }
                    //设置可取消
                    else if (self.GcOptions.Contains(info.behaviorOrder))
                    {
                        self.CancelableDict[info.behaviorOrder] = true;
                    }
                    else
                    {
                        self.CancelableDict[info.behaviorOrder] = false;
                    }
                }
            }
        }
    }
}