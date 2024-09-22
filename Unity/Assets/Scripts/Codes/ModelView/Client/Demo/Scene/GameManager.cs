namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class GameManager: Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static GameManager Instance;
    }

    public struct AfterCurrentSceneReload
    {
    }
}