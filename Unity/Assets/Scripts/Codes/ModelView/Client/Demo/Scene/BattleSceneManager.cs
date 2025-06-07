namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BattleSceneManager: Entity, IAwake, ILoad, IDestroy
    {
        [StaticField]
        public static BattleSceneManager Instance;
    }
}