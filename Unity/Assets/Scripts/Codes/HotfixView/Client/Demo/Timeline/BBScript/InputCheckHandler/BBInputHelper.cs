namespace ET.Client
{
    public static class BBInputHelper
    {
        public static InputWait GetInputWait(Unit unit)
        {
            return unit.GetComponent<TimelineComponent>().GetComponent<InputWait>();
        }

        public static BBTimerComponent GetBBTimer(Unit unit)
        {
            return unit.GetComponent<TimelineComponent>().GetComponent<BBTimerComponent>();
        }
    }
}