namespace ET.Client
{
    //优先级从上往下递增
    [UniqueId]
    public static class SkillOrder
    {
        public const int None = 0;
        public const int Move = 1;// dash idle run
        public const int Normal = 2; // 包括了嘲讽 parry throw 因为这些行为是组合键，优先级需要比一般拳脚高
        public const int SpecialMove = 3; 
        public const int SuperArt = 4;
    }
}