namespace ET.Client.V_Model
{
    public class DialogueReplaceAttribute: BaseAttribute
    {
    }

    //<作用是从数据层中获取数据
    //eg. 对话选项中显示玩家的能力 数值等等，
    //
    //以P5为例子，有的选项需要玩家 勇气达到5以上才能执行
    //我要在对话框中显示 4/5    4是查询玩家的数值组件 <Numeric type=Courage/>
    //                       5是当前对话节点中的数据，替换<CourageCheck/>为5 <---这个需要在对应的nodeHandler中处理(这个数据可能涉及更多的算法，比如玩家如果持有某些道具，所需的勇气值会比较低。总之，一定是逻辑层进行计算之后，将数据展现在表现层中)
    //                       <Numeric type=Courage/> / <CourageCheck/>
    [DialogueReplace]
    public abstract class ReplaceHandler
    {
        public abstract string GetReplaceType();
        public abstract string GetReplaceStr(Unit unit, string model);
    }
}