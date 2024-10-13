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

        #region InputCheckLogic

        public static bool FuzzyInput_Or(InputWait inputWait,InputCallback inputCallback, long op)
        {
            return false;
        }
        #endregion
    }
}