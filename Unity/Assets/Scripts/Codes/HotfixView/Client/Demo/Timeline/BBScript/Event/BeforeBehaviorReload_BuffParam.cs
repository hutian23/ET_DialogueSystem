﻿namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof (SkillBuffer))]
    public class BeforeBehaviorReload_BuffParam: AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            //把组件中的变量注册到BBParser中，然后其他reload
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            //1. 记录CurrentOrder
            bbParser.RegistParam("CurrentOrder", args.behaviorOrder);

            //2. 缓存TransitionFlag
            SkillBuffer buffer = timelineComponent.GetComponent<SkillBuffer>();
            foreach (string transitionFlag in buffer.transitionFlags)
            {
                bbParser.RegistParam($"Transition_{transitionFlag}", true);
            }

            //3. 清空GCOptions
            //TODO Clear Flag
            buffer.GCOptions.Clear();

            await ETTask.CompletedTask;
        }
    }
}