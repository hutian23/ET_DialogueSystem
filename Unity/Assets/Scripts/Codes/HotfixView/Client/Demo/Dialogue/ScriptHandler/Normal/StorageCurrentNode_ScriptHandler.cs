namespace ET.Client
{
    public class StorageCurrentNode : ScriptHandler
    {
        public override string GetOPType()
        {
            return "StorageCurrentNode";
        }

        public override async ETTask Handle(Unit unit, string line, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            
            // DialogueStorageManager.Instance.QuickSaveShot.AddToBuffer(dialogueComponent.currentNode);
            await ETTask.CompletedTask;
        }
    }
}