using Sirenix.OdinInspector;

namespace ET.Client
{
    [HideReferenceObjectPicker]
    public class NodeLinkData
    {
        public string outputNodeGuid;
        public string inputNodeGuid;
        [LabelText("输出节点的端口号")]
        public int portID;
    }
}