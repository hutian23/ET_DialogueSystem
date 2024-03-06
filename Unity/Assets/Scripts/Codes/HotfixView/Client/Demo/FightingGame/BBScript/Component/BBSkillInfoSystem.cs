namespace ET.Client
{
    [FriendOfAttribute(typeof (ET.Client.BBSkillInfo))]
    public static class BBSkillInfoSystem
    {
        public class BBSkillInfoAwakeSystem: AwakeSystem<BBSkillInfo, uint>
        {
            protected override void Awake(BBSkillInfo self, uint ID)
            {
                self.Id = ID;
            }
        }

        public static long GetSkillOrder(this BBSkillInfo self)
        {
            ulong result = 0;
            result |= self.order;
            result |= (ulong)self.skillType << 32;
            return (long)result;
        }
    }
}