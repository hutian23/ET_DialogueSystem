namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BattleSceneManager: Entity, IAwake, IDestroy
    {
        [StaticField]
        public static BattleSceneManager Instance;
    }
}