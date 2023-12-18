namespace ET.Client
{
    public class CheckerAttribute: BaseAttribute
    {
    }

    [Checker]
    public abstract class CheckerHandler
    {
        public abstract int Execute(Unit unit, CheckerConfig config);
    }
}