using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(LoopAnimComponent))]
    [FriendOf(typeof(BehaviorInfo))]
    public class Function_BeginLoopAnim_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BeginLoopAnim";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndLoopAnim:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;

            //1. 组件初始化
            Unit unit = parser.GetParent<Unit>();
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            TimelinePlayer timelinePlayer = timelineComponent.GetTimelinePlayer();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BehaviorInfo info = machine.GetInfoByOrder(machine.GetCurrentOrder());
            
            //2. 更新playableGraph
            BBTimeline _timeline = timelinePlayer.GetTimeline(info.behaviorName);
            if (_timeline == null)
            {
                Log.Error($"not found timeline name: {info.behaviorName}");
                return Status.Failed;
            }
            if (timelinePlayer.RuntimePlayable == null || timelinePlayer.RuntimePlayable.timeline != _timeline)
            {
                timelinePlayer.Init(_timeline);
            }

            parser.RemoveComponent<LoopAnimComponent>();
            LoopAnimComponent loopAnimComponent = parser.AddComponent<LoopAnimComponent>(true);
            loopAnimComponent.triggerIndex = startIndex;
            parser.RegistSubCoroutine(startIndex, endIndex, token).Coroutine();

            //3. 动画播放协程，阻塞技能协程 
            await loopAnimComponent.LoopAnimCor();
            
            return token.IsCancel() ? Status.Failed : Status.Success;
        }
    }
}