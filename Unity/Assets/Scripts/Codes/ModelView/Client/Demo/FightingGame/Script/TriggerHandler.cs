﻿namespace ET.Client
{
    public class TriggerAttribute: BaseAttribute
    {
    }

    [Trigger]
    public abstract class TriggerHandler
    {
        public abstract string GetTriggerType();

        public abstract bool Check(ScriptParser parser, ScriptData data);
    }
}