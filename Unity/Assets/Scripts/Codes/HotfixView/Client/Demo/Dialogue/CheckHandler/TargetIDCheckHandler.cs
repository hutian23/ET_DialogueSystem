
namespace ET.Client
{
    public class TargetIDCheckHandler: NodeCheckHandler<TargetIDCheckConfig>
    {
        protected override int Run(Unit unit, TargetIDCheckConfig nodeCheck)
        {
            int ret = 0;
            foreach (TargetCheck check in nodeCheck.checkList)
            {
                switch (check.CheckType)
                {
                    case TargetCheckType.已执行:
                        if (!DialogueStorageManager.Instance.QuickSaveShot.Check(check.treeID, check.targetID)) ret = 1;
                        break;
                    case TargetCheckType.未执行:
                        if (DialogueStorageManager.Instance.QuickSaveShot.Check(check.treeID, check.targetID)) ret = 1;
                        break;
                }
            }
            return ret;
        }
    }
}