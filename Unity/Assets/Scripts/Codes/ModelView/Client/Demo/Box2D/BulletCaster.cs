namespace ET.Client
{
    [ComponentOf(typeof(b2Body))]
    public class BulletCaster: Entity, IAwake<long, int>, IDestroy
    {
        // Caster.InstanceId
        public long _instanceId;
        
        // 默认情况为FilterType.BulletHitFilter, 即不会与子弹的发射者碰撞
        // 类似GGST中浮士德的炸弹，施放者同样会被打击，需要传入不同的FilterType
        public int filterType;  
    }
}