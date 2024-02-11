using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace ET.Client
{
    public class InspectorDataView: SerializedScriptableObject
    {
        [ShowInInspector]
        [OdinSerialize, LabelText("选择节点"), HideReferenceObjectPicker]
        [ListDrawerSettings(IsReadOnly = true)]
        public HashSet<DialogueNode> datas = new HashSet<DialogueNode>();
    }

    public class ShareVariableView: SerializedScriptableObject
    {
        [OdinSerialize, LabelText("共享变量")]
        [ListDrawerSettings(ShowFoldout = true, ShowIndexLabels = true, ListElementLabelName = "name", IsReadOnly = true), ReadOnly]
        public List<SharedVariable> SharedVariables = new() { new SharedVariable() { name = "Hello world", value = 1 } };
    }
}