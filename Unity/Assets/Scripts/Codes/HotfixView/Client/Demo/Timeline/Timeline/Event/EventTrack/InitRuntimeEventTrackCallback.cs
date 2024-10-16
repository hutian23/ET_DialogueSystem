﻿using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (TimelineEventManager))]
    [FriendOf(typeof (ScriptParser))]
    public class InitEventTrackCallback: AInvokeHandler<InitEventTrack>
    {
        public override void Handle(InitEventTrack args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null) return;

            BBEventTrack track = args.RuntimeEventTrack.Track as BBEventTrack;

            //runtime event track ---> scriptParser
            Unit unit = timelineComponent.GetParent<Unit>();
            TimelineEventManager manager = timelineComponent.GetComponent<TimelineEventManager>();

            switch (args.initType)
            {
                //Bind
                case 0:
                {
                    if (manager.parserDict.TryGetValue(track.Name, out long id))
                    {
                        manager.RemoveChild(id);
                        manager.parserDict.Remove(track.Name);
                    }

                    ScriptParser parser = manager.AddChild<ScriptParser, long>(unit.InstanceId);
                    manager.parserDict.Add(track.Name, parser.Id);
                    break;
                }
                //UnBind
                case 1:
                {
                    if (manager.parserDict.TryGetValue(track.Name, out long id))
                    {
                        manager.RemoveChild(id);
                        manager.parserDict.Remove(track.Name);
                    }

                    break;
                }
            }
        }
    }
}