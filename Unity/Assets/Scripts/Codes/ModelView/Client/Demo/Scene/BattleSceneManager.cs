namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BattleSceneManager: Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static BattleSceneManager Instance;
    }
}