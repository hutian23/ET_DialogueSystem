﻿namespace ET.Client
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
                dialogueComponent.RemoveComponent<TODTimerComponent>();
                dialogueComponent.RemoveComponent<BBParser>();
                dialogueComponent.RemoveComponent<GatlingCancel>();
            });

            dialogueComponent.AddComponent<TODTimerComponent>();
            dialogueComponent.AddComponent<BBParser>();
            dialogueComponent.AddComponent<GatlingCancel>();

            await ETTask.CompletedTask;
        }
    }
}