namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class VirtualCameraManager: Entity, IAwake, IDestroy, ILoad, IFrameLateUpdate
    {
        [StaticField]
        public static VirtualCameraManager Instance;
    }

    public struct UpdateFollowOffsetCallback
    {
        public long instanceId; // bbParser.instanceId
        public int flip; // 朝向
    }

    public struct CameraTarget
    {
        public long instanceId;
        public float weight;
        public float radius;
    }
}