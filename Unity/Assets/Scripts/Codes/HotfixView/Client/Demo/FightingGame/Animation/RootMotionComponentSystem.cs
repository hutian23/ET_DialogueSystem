using Timeline;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (RootMotionComponent))]
    public static class RootMotionComponentSystem
    {
        public class RootMotionComponentAwakeSystem: AwakeSystem<RootMotionComponent>
        {
            protected override void Awake(RootMotionComponent self)
            {
            }
        }

        public static void Init(this RootMotionComponent self, long targetID)
        {
            GameObject go = self.GetParent<DialogueComponent>().GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            TimelinePlayer timelinePlayer = go.GetComponent<TimelinePlayer>();

            self.initPos = timelinePlayer.transform.position;
        }

        public static void UpdatePos(this RootMotionComponent self, int targetFrame)
        {
            PlayableManager manager = self.GetParent<DialogueComponent>().GetComponent<PlayableManager>();
            RuntimePlayable runtimePlayable = manager.GetPlayable();

            if (!runtimePlayable.TimelinePlayer.ApplyRootMotion)
            {
                return;
            }

            foreach (var runtimeTrack in runtimePlayable.RuntimeTracks)
            {
                if (runtimeTrack is not RuntimeAnimationTrack runtimeAnimationTrack)
                {
                    continue;
                }

                BBAnimationTrack track = runtimeAnimationTrack.AnimationTrack;

                foreach (var clip in track.Clips)
                {
                    if (clip.InMiddle(targetFrame))
                    {
                        if (self.currentClip != clip)
                        {
                            self.OnDone();
                        }

                        self.currentClip = clip as BBAnimationClip;
                        int ClipInFrame = targetFrame - self.currentClip.StartFrame;
                        //Update position
                        //TODO 物理模拟
                        self.GetTransform().position = self.initPos + self.currentClip.CurrentPosition(ClipInFrame);
                        return;
                    }
                }

                self.OnDone();
                return;
            }

            //TODO 
            //1. currentframe所在animationClip
            //3. 当前帧位移量
        }

        //当前clip执行完的回调
        public static void OnDone(this RootMotionComponent self)
        {
            self.currentClip = null;
            self.initPos = self.GetTransform().position;
        }

        private static Transform GetTransform(this RootMotionComponent self)
        {
            TimelinePlayer timelinePlayer = self.GetParent<DialogueComponent>().GetComponent<PlayableManager>().GetPlayable().TimelinePlayer;
            return timelinePlayer.transform;
        }
    }
}