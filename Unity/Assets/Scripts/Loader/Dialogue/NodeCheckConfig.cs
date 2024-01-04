namespace ET.Client
{
    public class NodeCheckConfig
    {
        public string CheckerName;
    }

    public class NumericLessThanCofig: NodeCheckConfig
    {
        public int NumericType;
        public int minValue;
    }
}