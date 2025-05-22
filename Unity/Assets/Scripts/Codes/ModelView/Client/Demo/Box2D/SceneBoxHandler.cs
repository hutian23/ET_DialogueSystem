namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class SceneBoxHandler : Entity, IAwake, IDestroy, IPostStep, IGizmosUpdate
    {
        // public CollisionInfo info;
        public long unitId;
    }
}