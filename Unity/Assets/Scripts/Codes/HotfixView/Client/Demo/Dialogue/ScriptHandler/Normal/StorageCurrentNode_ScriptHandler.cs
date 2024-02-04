namespace ET.Client
{
    [FriendOf(typeof (DialogueComponent))]
    public class StorageCurrentNode: ScriptHandler
    {
        public override string GetOPType()
        {
            return "StorageCurrentNode";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            DialogueStorageManager.Instance.QuickSaveShot.AddToBuffer(node);
            await ETTask.CompletedTask;
        }
    }
}