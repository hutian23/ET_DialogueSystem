using UnityEngine;

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
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;

            token.Add(() =>
            {
                dialogueComponent.RemoveComponent<BBTimerComponent>();
                dialogueComponent.RemoveComponent<BBParser>();
                dialogueComponent.RemoveComponent<BBInputComponent>();
                dialogueComponent.RemoveComponent<BehaviorBufferComponent>();
                unit.RemoveComponent<NumericComponent>();

                if (go != null)
                {
                    BBTestManager testManager = go.GetComponent<BBTestManager>();
                    UnityEngine.Object.DestroyImmediate(testManager);
                }
            });

            dialogueComponent.AddComponent<BBTimerComponent>();
            dialogueComponent.AddComponent<BBParser>();
            dialogueComponent.AddComponent<BBInputComponent>();
            dialogueComponent.AddComponent<BehaviorBufferComponent>();
            unit.AddComponent<NumericComponent>();

            BBTestManager testManager = go.AddComponent<BBTestManager>();
            testManager.instanceId = dialogueComponent.InstanceId;
            
            await ETTask.CompletedTask;
        }
    }
}