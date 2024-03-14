using UnityEngine.UIElements;

namespace ET.Client
{
    public sealed class BBNodeView: DialogueNodeView<BBNode>
    {
        public BBNodeView(BBNode dialogueNode, DialogueTreeView dialogueTreeView): base(dialogueNode, dialogueTreeView)
        {
            GenerateInputPort("", true);
            GenerateOutputPort("", true);

            Button btn = new(() => { BBScriptEditor.Init(dialogueNode); }) { text = "打开编辑器" };
            btn.AddToClassList("Btn");
            this.title =$"[{dialogueNode.TargetID}] {dialogueNode.behaviorName}";
            
            this.contentContainer.Add(btn);
        }
    }
}