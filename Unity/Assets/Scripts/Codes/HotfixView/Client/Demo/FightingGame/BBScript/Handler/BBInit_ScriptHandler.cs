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
                dialogueComponent.RemoveComponent<BBInputComponent>();
                dialogueComponent.RemoveComponent<BehaviorBufferComponent>();
                dialogueComponent.RemoveComponent<BBAnimComponent>();
                unit.RemoveComponent<NumericComponent>();
            });

            dialogueComponent.AddComponent<BBTimerComponent>();
            dialogueComponent.AddComponent<BBParser>();
            dialogueComponent.AddComponent<BBInputComponent>();
            dialogueComponent.AddComponent<BehaviorBufferComponent>();
            dialogueComponent.AddComponent<BBAnimComponent>();
            unit.AddComponent<NumericComponent>();

            await ETTask.CompletedTask;
        }
    }
}