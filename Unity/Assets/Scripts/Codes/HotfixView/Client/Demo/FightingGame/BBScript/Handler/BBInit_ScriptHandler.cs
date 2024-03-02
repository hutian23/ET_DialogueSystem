namespace ET.Client
{
    public class BBInit_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "BBInit";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();

            token.Add(() =>
            {
                dialogueComponent.RemoveComponent<BBTimerComponent>();
                dialogueComponent.RemoveComponent<BBParser>();
                dialogueComponent.RemoveComponent<GatlingCancel>();
                dialogueComponent.RemoveComponent<BBInputComponent>();
            });

            dialogueComponent.AddComponent<BBTimerComponent>();
            dialogueComponent.AddComponent<BBParser>();
            dialogueComponent.AddComponent<GatlingCancel>();
            dialogueComponent.AddComponent<BBInputComponent>();
            
            await ETTask.CompletedTask;
        }
    }
}