using System.Linq;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof (SkillInfo))]
    [FriendOf(typeof (SkillBuffer))]
    [FriendOf(typeof (BBTimerComponent))]
    public static class SkillBufferSystem
    {
        [Invoke(BBTimerInvokeType.SkillCheckTimer)]
        [FriendOf(typeof (SkillBuffer))]
        [FriendOf(typeof (SkillInfo))]
        public class SkillCheckTimer: BBTimer<SkillBuffer>
        {
            protected override void Run(SkillBuffer self)
            {
                foreach (long id in self.Ids)
                {
                    SkillInfo info = self.GetChild<SkillInfo>(id);
                    //已经进入当前行为，不会重复检查进入条件
                    //比当前行为权值小的行为也不会进行检查
                    if (info.order == self.currentOrder)
                    {
                        break;
                    }

                    var ret = info.SkillCheck();
                    if (ret)
                    {
                        if (self.currentOrder != info.order)
                        {
                            self.GetParent<TimelineComponent>().Reload(info.order);
                        }
                        break;
                    }
                }
            }
        }

        public static void Reload(this SkillBuffer self)
        {
            foreach (long id in self.Ids)
            {
                self.RemoveChild(id);
            }

            self.Ids.Clear();
            self.SetTransition(string.Empty);
            self.currentOrder = -1;

            var timelines = self.GetParent<TimelineComponent>()
                    .GetTimelinePlayer().BBPlayable
                    .GetTimelines()
                    .ToList().OrderByDescending(timeline => timeline.order);

            //根据behaviorOrder进行排序(降序)
            //权值越高的行为越前检查
            foreach (BBTimeline timeline in timelines)
            {
                SkillInfo info = self.AddChild<SkillInfo>();
                info.LoadSkillInfo(timeline);
                self.Ids.Add(info.Id);
            }

            //启动检测定时器
            BBTimerComponent timerComponent = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            timerComponent.Remove(ref self.CheckTimer);
            self.CheckTimer = timerComponent.NewFrameTimer(BBTimerInvokeType.SkillCheckTimer, self);
        }

        public static void ClearFlag(this SkillBuffer self)
        {
            self.flags.Clear();
        }

        public static void RemoveFlag(this SkillBuffer self, string flag)
        {
            self.flags.Remove(flag);
        }

        public static void AddFlag(this SkillBuffer self, string flag)
        {
            self.flags.Add(flag);
        }

        public static bool ContainFlag(this SkillBuffer self, string flag)
        {
            return self.flags.Contains(flag);
        }

        public static void SetTransition(this SkillBuffer self, string trans)
        {
            self.TransitionFlag = trans;
        }

        public static string GetTransition(this SkillBuffer self)
        {
            return self.TransitionFlag;
        }

        public static void SetCurrentOrder(this SkillBuffer self, int order)
        {
            self.currentOrder = order;
        }

        public static int GetCurrentOrder(this SkillBuffer self)
        {
            return self.currentOrder;
        }
    }
}