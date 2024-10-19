using System.Linq;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof (SkillInfo))]
    [FriendOf(typeof (SkillBuffer))]
    [FriendOf(typeof (BBTimerComponent))]
    public static class SkillBufferSystem
    {
        [Invoke(BBTimerInvokeType.BehaviorCheckTimer)]
        [FriendOf(typeof (SkillBuffer))]
        [FriendOf(typeof (SkillInfo))]
        public class SkillCheckTimer: BBTimer<SkillBuffer>
        {
            protected override void Run(SkillBuffer self)
            {
                foreach (var kv in self.infoDict)
                {
                    SkillInfo info = self.GetChild<SkillInfo>(kv.Value);
                    //已经进入当前行为，不会重复检查进入条件
                    //比当前行为权值小的行为也不会进行检查
                    if (info.order == self.currentOrder)
                    {
                        break;
                    }

                    bool ret = info.SkillCheck();
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

        [Invoke(BBTimerInvokeType.GatlingCancelCheckTimer)]
        [FriendOf(typeof (SkillBuffer))]
        [FriendOf(typeof (SkillInfo))]
        public class GatlingCancelCheckTimer: BBTimer<SkillBuffer>
        {
            protected override void Run(SkillBuffer self)
            {
                SkillInfo curInfo = self.GetInfo(self.currentOrder);

                foreach (var kv in self.infoDict)
                {
                    SkillInfo info = self.GetChild<SkillInfo>(kv.Value);
                    if (info.behaviorOrder == curInfo.behaviorOrder)
                    {
                        continue;
                    }

                    //1. 加特林取消不能取消到该行为
                    bool ret = (info.moveType > curInfo.moveType) || self.ContainGCOption(info.behaviorOrder);
                    if (!ret)
                    {
                        continue;
                    }

                    //2. 检查先置条件
                    ret = info.SkillCheck();
                    if (ret)
                    {
                        if (self.currentOrder != info.behaviorOrder)
                        {
                            self.GetParent<TimelineComponent>().Reload(info.behaviorOrder);
                        }

                        break;
                    }
                }
            }
        }

        public static void Reload(this SkillBuffer self)
        {
            foreach (var kv in self.infoDict)
            {
                self.RemoveChild(kv.Value);
            }

            self.infoDict.Clear();
            self.GCOptions.Clear();
            self.ClearParam();
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
                if (!self.infoDict.TryAdd(timeline.order, info.Id))
                {
                    Log.Error($"Already exist Behavior order: {timeline.order} --- {timeline.name}");
                    return;
                }
            }

            //启动检测定时器
            BBTimerComponent timerComponent = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            timerComponent.Remove(ref self.CheckTimer);
            self.CheckTimer = timerComponent.NewFrameTimer(BBTimerInvokeType.BehaviorCheckTimer, self);
        }

        public static void SetCurrentOrder(this SkillBuffer self, int order)
        {
            self.currentOrder = order;
        }

        public static int GetCurrentOrder(this SkillBuffer self)
        {
            return self.currentOrder;
        }

        public static SkillInfo GetInfo(this SkillBuffer self, int order)
        {
            if (!self.infoDict.TryGetValue(order, out long id))
            {
                Log.Error($"does not exist skillInfo:{order}");
                return null;
            }

            return self.GetChild<SkillInfo>(id);
        }

        #region Param

        public static T RegistParam<T>(this SkillBuffer self, string paramName, T value)
        {
            if (self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"already contain params:{paramName}");
                return default;
            }

            SharedVariable variable = SharedVariable.Create(paramName, value);
            self.paramDict.Add(paramName, variable);
            return value;
        }

        public static T GetParam<T>(this SkillBuffer self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return default;
            }

            SharedVariable variable = self.paramDict[paramName];
            if (variable.value is not T value)
            {
                Log.Error($"cannot format {variable.name} to {typeof (T)}");
                return default;
            }

            return value;
        }

        public static bool ContainParam(this SkillBuffer self, string paramName)
        {
            return self.paramDict.ContainsKey(paramName);
        }

        public static void RemoveParam(this SkillBuffer self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return;
            }

            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
        }

        public static void ClearParam(this SkillBuffer self)
        {
            foreach (var kv in self.paramDict)
            {
                self.paramDict[kv.Key].Recycle();
            }

            self.paramDict.Clear();
        }

        #endregion

        #region GCOption

        public static void AddGCOption(this SkillBuffer self, string behaviorName)
        {
            foreach (var kv in self.infoDict)
            {
                SkillInfo info = self.GetChild<SkillInfo>(kv.Value);
                if (info.behaviorName.Equals(behaviorName))
                {
                    self.GCOptions.Add(info.behaviorOrder);
                    return;
                }
            }

            Log.Error($"does not exist behavior: {behaviorName}");
        }

        private static bool ContainGCOption(this SkillBuffer self, int behaviorOrder)
        {
            return self.GCOptions.Contains(behaviorOrder);
        }

        public static bool ContainGCOption(this SkillBuffer self, string behaviorName)
        {
            int targetOrder = -1;
            foreach (var kv in self.infoDict)
            {
                SkillInfo info = self.GetChild<SkillInfo>(kv.Value);
                if (info.behaviorName.Equals(behaviorName))
                {
                    targetOrder = info.behaviorOrder;
                }
            }

            return self.ContainGCOption(targetOrder);
        }

        #endregion
    }
}