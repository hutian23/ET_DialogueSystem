namespace ET.Client.V_Model
{
    public class DialogueModelAttribute: BaseAttribute
    {
    }
    
    //<model/>的作用是从数据层中获取数据
    //eg. 对话选项中显示玩家的能力 数值等等，
    //
    //以P5为例子，有的选项需要玩家 勇气达到5以上才能执行
    //我要在对话框中显示 4/5    4是查询玩家的数值组件 <model type=Numeric name=Courage/>
    //                       5是当前对话节点中的数据，可以使用类似vue中的双括号插值的形式 {{CourageCheck}} <---这个需要在对应的nodeHandler中处理
    //                       <model type=Numeric name=Courage/> / {{CourageCheck}}
    [DialogueModel]
    public abstract class ModelHandler
    {
        public abstract string GetModelType();
        public abstract string GetReplaceStr(Unit unit, string model);
    }
}