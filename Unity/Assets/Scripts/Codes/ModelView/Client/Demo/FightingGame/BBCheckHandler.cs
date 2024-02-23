using System;

namespace ET.Client
{
    public class BBScriptAttribute: BaseAttribute
    {
    }

    [BBScript]
    public abstract class BBCheckHandler
    {
        public abstract string GetSkillType();

        public abstract ETTask Handler(Unit unit, ETCancellationToken token);
    }
}