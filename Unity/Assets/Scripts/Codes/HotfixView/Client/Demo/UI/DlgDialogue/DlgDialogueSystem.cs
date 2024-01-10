namespace ET.Client
{
    [FriendOf(typeof(DlgDialogue))]
    [FriendOf(typeof(DialogueStorage))]
    public static class DlgDialogueSystem
    {

        public static void RegisterUIEvent(this DlgDialogue self)
        {
            self.View.E_ClearQSButton.AddListener(self.ClearQuickSave);
            self.View.E_CheckQuickSaveButton.AddListener(self.CheckQS);
        }
        
        public class DlgDialogueLoadSystem : LoadSystem<DlgDialogue>
        {
            protected override void Load(DlgDialogue self)
            {
                self.View.E_ClearQSButton.AddListener(self.ClearQuickSave);
                self.View.E_CheckQuickSaveButton.AddListener(self.CheckQS);
            }
        }
        
        public static void ShowWindow(this DlgDialogue self, Entity contextData = null)
        {
        }

        private static void ClearQuickSave(this DlgDialogue self)
        {
            DialogueStorageManager.Instance.QuickSaveShot.ClearBuffer();
            DialogueStorageManager.Instance.QuickSaveShot.storageSet.Clear();
        }

        private static void CheckQS(this DlgDialogue self)
        {
            uint treeID = uint.Parse(self.View.E_CheckInput_TreeIDInputField.text);
            uint targetID = uint.Parse(self.View.E_CheckInput_TargetIDInputField.text);
            Log.Warning($"该节点是否已经执行?: " + DialogueStorageManager.Instance.QuickSaveShot.Check(treeID, targetID));
        }
    }
}
