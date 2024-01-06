using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace ET.Client
{
    public class InspectorDataView: SerializedScriptableObject
    {
        [ShowInInspector]
        [OdinSerialize, LabelText("选择的点"), HideReferenceObjectPicker]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<DialogueNode> datas=new List<DialogueNode>(){new RootNode(),new Angry_ChoiceNode(),new BubbleActionNode()};
    }
}