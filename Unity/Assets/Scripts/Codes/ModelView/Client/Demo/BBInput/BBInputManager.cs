namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BBInputManager: Entity, IAwake, IDestroy, IUpdate
    {
        [StaticField]
        public static BBInputManager Instance;
    }
}