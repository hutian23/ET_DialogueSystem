namespace ET
{
    // 子弹时间时，会根据UnitType选择是否缩放指定Unit的TimeScale
    public enum UnitType: byte
    {
        None = 0,
        Player = 1,
        Monster = 2,
        NPC = 3,
    }
}