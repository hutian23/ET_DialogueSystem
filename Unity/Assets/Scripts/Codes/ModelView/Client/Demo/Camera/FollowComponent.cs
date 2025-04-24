namespace ET.Client
{
    [ComponentOf(typeof(VirtualCameraManager))]
    public class FollowComponent : Entity, IAwake<long>, IDestroy
    {
        public long _instanceId;
        public ETCancellationToken token;
    }
}