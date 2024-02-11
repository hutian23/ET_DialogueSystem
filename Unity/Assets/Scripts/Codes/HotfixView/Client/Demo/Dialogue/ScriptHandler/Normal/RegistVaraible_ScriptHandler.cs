using System.Linq;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(DialogueComponent))]
    public class RegistVaraible_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistVariable";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            // dialogueComponent.Variables.Clear();
            // dialogueComponent.Variables.Add("hutian", new SharedVariable() { name = "hutian222", value = 5.5 });
            // if (Application.isEditor)
            // {
            //     unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<DialogueViewComponent>().Variables.Add(new SharedVariable(){name = "hutian",value = 111});
            // }
            dialogueComponent.Variables.Add(new SharedVariable(){name = "hutian",value = 2132323});
            await ETTask.CompletedTask;
        }
    }
}