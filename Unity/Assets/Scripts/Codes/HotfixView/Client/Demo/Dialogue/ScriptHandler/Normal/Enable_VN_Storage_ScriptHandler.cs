namespace ET.Client
{
    public class Enable_VN_Storage_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "Enable_VN_Storage";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            DialogueStorageManager.Instance.QuickSaveShot.RemoveComponent<VN_Storage>();
            DialogueStorageManager.Instance.QuickSaveShot.AddComponent<VN_Storage>();
            await ETTask.CompletedTask;
        }
    }
}