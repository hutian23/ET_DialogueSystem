using Timeline.Editor;

namespace Timeline
{
    // Timeline 记录keyframe
    // keyframe应该包含什么信息?
    // 1. Sprite -- 对应AnimationClip
    // 2. Hitbox
    // 3. targetbind -- Vector3
    // 4. particle -- position rotation
    // 5. sound 是否需要添加到keyframe中？ 感觉不需要
    
    [BBTrack("SubTimeline")]
#if UNITY_EDITOR
    [Color(100, 100, 100)]
    [IconGuid("799823b53d556d34faeb55e049c91845")]
#endif
    public class SubTimelineTrack: BBTrack
    {
    }
}