namespace ET.Client
{
    public static class InputHelper
    {
        public static long GetBuffFrame(this InputComponent self, int buffFrame)
        {
            return self.GetParent<Unit>().GetComponent<BBTimerComponent>().GetNow() + buffFrame;
        }
    }
}