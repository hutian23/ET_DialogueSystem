namespace ET.Client
{
    [ChildOf(typeof(BBNumeric))]
    public class NumericCallback : Entity, IAwake, IDestroy
    {
        public int startIndex;
        public int endIndex;
        public string NumericType;
    }
}
