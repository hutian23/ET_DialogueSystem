namespace ET.Client
{
    // unit死亡时，销毁这个buff
    [ComponentOf(typeof(BuffManager))]
    public class DeathAbility : Entity, IAwake, IDestroy
    {
    }
}