using System;

namespace ET.Client
{
    public class BBScriptAttribute: BaseAttribute
    {
    }

    [BBScript]
    public abstract class BBCheckHandler: NodeCheckHandler
    {
        public int Check(Unit unit, object nodeCheckConfig)
        {
            return 0;
        }

        public Type GetNodeCheckType()
        {
            return default;
        }

        public abstract string GetSkillType();

        public abstract int Run(Unit unit);
    }
}