using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (DlgStorage))]
    [FriendOf(typeof (DialogueStorageManager))]
    [FriendOf(typeof (DialogueStorage))]
    [FriendOf(typeof (DlgStorageViewComponent))]
    [FriendOf(typeof (Scroll_Item_Storage))]
    public static class DlgStorageSystem
    {
        public static void RegisterUIEvent(this DlgStorage self)
        {
            self.View.E_StorageLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopStorageRefreshHandler);
        }

        public static void ShowWindow(this DlgStorage self, Entity contextData = null)
        {
        }

        private static void OnLoopStorageRefreshHandler(this DlgStorage self, Transform transform, int index)
        {
            //注意第一位是快照，不显示在UI中
            DialogueStorage storage = DialogueStorageManager.Instance.GetByIndex(index + 1);
            Scroll_Item_Storage scrollItemStorage = self.ScrollItemStorages[index].BindTrans(transform);

            //空存档
            if (storage.storageSet.Count == 0)
            {
                scrollItemStorage.uiTransform.Find("E_Content").gameObject.SetActive(false);
                scrollItemStorage.uiTransform.Find("E_EmptySlot").gameObject.SetActive(true);
                return;
            }

            ulong tmpID = (ulong)storage.currentID;
            uint targetID = (uint)(tmpID & uint.MaxValue);
            tmpID >>= 32;
            uint treeID = (uint)(tmpID & uint.MaxValue);

            scrollItemStorage.E_ContentText.SetText($"存档{index + 1}");
            scrollItemStorage.E_TreeIDText.SetText(treeID.ToString());
            scrollItemStorage.E_TargetIDText.SetText(targetID.ToString());
            scrollItemStorage.E_SelectButton.AddListener(() =>
            {
                TODUnitHelper.GetPlayer(self.ClientScene())
                        .GetComponent<DialogueComponent>()
                        .GetComponent<ObjectWait>()
                        .Notify(new WaitSelectStorageSlot() { index = index + 1 });
            });
        }

        public static void Refresh(this DlgStorage self)
        {
            self.View.E_StorageLoopVerticalScrollRect.RefreshCells();
            self.AddUIScrollItems(ref self.ScrollItemStorages, DialogueStorageManager.MaxSize - 1);
            self.View.E_StorageLoopVerticalScrollRect.SetVisible(true, DialogueStorageManager.MaxSize - 1);

            self.HideConfirmPanel();
        }

        private static void HideConfirmPanel(this DlgStorage self)
        {
            self.View.uiTransform.Find("E_Confirm").gameObject.SetActive(false);
        }
    }
}