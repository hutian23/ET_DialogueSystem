namespace ET.Client
{
    public class NumeircCheckHandler: NodeCheckHandler<NumericCheckConfig>
    {
        protected override int Run(Unit unit, NumericCheckConfig nodeCheck)
        {
            Unit player = TODUnitHelper.GetPlayer(unit.ClientScene());
            int numeric = player.GetComponent<NumericComponent>().GetAsInt(nodeCheck.NumericType);
            switch (nodeCheck.CheckType)
            {
                case NumericCheckerType.Equal:
                    return (numeric == nodeCheck.EqualValue)? 0 : 1;
                case NumericCheckerType.InRange:
                    return (numeric >= nodeCheck.minValue && numeric <= nodeCheck.maxValue)? 0 : 1;
                case NumericCheckerType.LessThan:
                    return (numeric < nodeCheck.EqualValue)? 0 : 1;
                case NumericCheckerType.MoreThan:
                    return (numeric > nodeCheck.EqualValue)? 0 : 1;
                default:
                    return 1;
            }
        }
    }
}