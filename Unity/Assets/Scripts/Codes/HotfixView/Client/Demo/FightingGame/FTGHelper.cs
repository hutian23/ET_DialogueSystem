using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (BBInputComponent))]
    [FriendOf(typeof (BBTimerComponent))]
    public static class FTGHelper
    {
        public static List<int> GetInput(long ops)
        {
            var tmpList = new List<int>();
            for (int i = 0; i < 64; i++)
            {
                if ((ops & (2 << i)) != 0)
                {
                    tmpList.Add(i);
                }
            }

            return tmpList;
        }

        //获取输入缓冲组件当前帧号
        public static long GetCurFrame(Unit unit)
        {
            return unit.GetComponent<DialogueComponent>().GetComponent<BBInputComponent>().GetComponent<BBTimerComponent>().curFrame;
        }

        public static BBWait GetBBWait(Unit unit)
        {
            return unit.GetComponent<DialogueComponent>().GetComponent<BBInputComponent>().GetComponent<BBWait>();
        }

        public static BBSkillInfo GetSkillInfo(Unit unit, uint targetID)
        {
            BBInputComponent inputComponent = unit.GetComponent<DialogueComponent>().GetComponent<BBInputComponent>();
            return inputComponent.GetChild<BBSkillInfo>(targetID);
        }
    }
}