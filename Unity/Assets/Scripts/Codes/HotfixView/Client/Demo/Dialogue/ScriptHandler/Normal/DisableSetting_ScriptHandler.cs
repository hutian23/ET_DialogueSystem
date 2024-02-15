﻿namespace ET.Client
{
    public class DisableSetting : ScriptHandler
    {
        public override string GetOPType()
        {
            return "DisableSetting";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            unit.GetComponent<DialogueComponent>().RemoveComponent<SettingOpera>();
            await ETTask.CompletedTask;
        }
    }
}