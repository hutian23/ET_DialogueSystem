﻿namespace ET.Client
{
    public class EnableSetting_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableSetting";
        }

        //EnableSetting;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
        }
    }
}