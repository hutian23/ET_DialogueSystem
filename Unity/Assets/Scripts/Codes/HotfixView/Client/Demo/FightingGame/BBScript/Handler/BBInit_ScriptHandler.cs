namespace ET.Client
{
    public class BBInit_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "BBInit";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            dialogueComponent.RemoveComponent<TODTimerComponent>();
            dialogueComponent.AddComponent<TODTimerComponent>();
            await ETTask.CompletedTask;
        }
    }
}