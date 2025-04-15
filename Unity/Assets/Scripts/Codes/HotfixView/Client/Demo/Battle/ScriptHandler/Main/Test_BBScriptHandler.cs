using MongoDB.Bson;
using Timeline;

namespace ET.Client
{
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            foreach (RuntimeTrack runtimeTrack in parser.GetParent<Unit>().GetComponent<TimelineComponent>().GetTimelinePlayer().RuntimePlayable.RuntimeTracks)
            {
                Log.Warning(runtimeTrack.Track.ToJson());
            }
            Log.Warning(parser.GetParent<Unit>().GetComponent<TimelineComponent>().GetTimelinePlayer().RuntimePlayable.RuntimeTracks.Count.ToString());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}