using UnityEngine.UIElements;

namespace ET.Client
{
    public sealed class BBNodeView: DialogueNodeView<BBNode>
    {
        public BBNodeView(BBNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            GenerateOutputPort("", true);
            this.contentContainer.Add(new Button(() => { BBScriptEditor.Init(dialogueNode); }) { text = "打开编辑器" });
        }
    }
}