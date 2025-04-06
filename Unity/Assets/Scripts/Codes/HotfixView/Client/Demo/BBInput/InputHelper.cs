namespace ET.Client
{
    public static class InputHelper
    {
        public static long GetBuffFrame(this InputWait self, int buffFrame)
        {
            // return self.GetParent<Unit>().GetComponent<BBTimerComponent>().GetNow() + buffFrame;
            return BBTimerManager.Instance.SceneTimer().GetNow() + buffFrame;
        }
    }
}