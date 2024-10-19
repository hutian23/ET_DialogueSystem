using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(SkillBuffer))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(SkillInfo))]
    public class HandleUpdateSkillBufferCallback : AInvokeHandler<ReloadSkillBufferCallback>
    {
        public override void Handle(ReloadSkillBufferCallback args)
        {
            SkillBuffer buffer = Root.Instance.Get(args.instanceId) as SkillBuffer;
            BBParser parser = buffer.GetParent<TimelineComponent>().GetComponent<BBParser>();

            foreach (var kv in buffer.infoDict)
            {
                buffer.RemoveChild(kv.Value);
            }

            buffer.infoDict.Clear();
            buffer.GCOptions.Clear();
            buffer.ClearParam();
            buffer.behaviorMap.Clear();
            buffer.currentOrder = -1;

            var timelines = buffer.GetParent<TimelineComponent>()
                    .GetTimelinePlayer().BBPlayable
                    .GetTimelines();

            foreach (BBTimeline timeline in timelines)
            {
                SkillInfo info = buffer.AddChild<SkillInfo>();

                //1. 加载Trigger
                info.LoadSkillInfo(timeline);

                //2. 初始化协程
                parser.InitScript(timeline.Script);
                parser.RegistParam("InfoId", info.Id);
                parser.Invoke("Init", parser.cancellationToken).Coroutine();
            }

            //3. 按照优先级注册行为到InfoDict中,权值越高的行为越前检查
            foreach (Entity child in buffer.Children.Values)
            {
                SkillInfo info = child as SkillInfo;
                buffer.infoDict.Add(info.behaviorOrder, info.Id);
                buffer.behaviorMap.Add(info.behaviorName, info.behaviorOrder);
            }
            
            //4. 启动检测定时器
            BBTimerComponent bbTimer = buffer.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref buffer.CheckTimer);
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.BehaviorCheckTimer, buffer);
        }
    }
}