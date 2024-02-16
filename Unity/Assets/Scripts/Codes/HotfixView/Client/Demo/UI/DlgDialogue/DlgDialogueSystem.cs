using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ET.Client
{
    [FriendOf(typeof (DlgDialogue))]
    [FriendOf(typeof (DialogueStorage))]
    public static class DlgDialogueSystem
    {
        public static void RegisterUIEvent(this DlgDialogue self)
        {
            self.View.E_ClearQSButton.AddListener(self.ClearQuickSave);
            self.View.E_CheckQuickSaveButton.AddListener(self.CheckQS);
            self.View.E_SaveButton.AddListenerAsync(self.Save);
            self.View.E_LoadButton.AddListenerAsync(self.Load);
            self.View.E_ChoicePanelLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopChoiceRefreshHandler);
            self.View.E_TestButton.AddListener(self.Test);
            self.RefreshArrow();
        }

        public static void ShowWindow(this DlgDialogue self, Entity contextData = null)
        {
        }

        private static void ClearQuickSave(this DlgDialogue self)
        {
            DialogueStorageManager.Instance.QuickSaveShot.ClearBuffer();
            DialogueStorageManager.Instance.QuickSaveShot.storageSet.Clear();
        }

        private static async ETTask Save(this DlgDialogue self)
        {
            DialogueComponent dialogueComponent = TODUnitHelper.GetPlayer(self.ClientScene()).GetComponent<DialogueComponent>();
            if (!dialogueComponent.ContainTag(DialogueTag.CanEnterSetting)) return;

            dialogueComponent.RemoveTag(DialogueTag.InDialogueCor);
            await self.ClientScene().GetComponent<UIComponent>().ShowWindowAsync<DlgStorage>();
            self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgStorage>().Refresh();

            ETCancellationToken escapeToken = new();
            self.EscapeCor(escapeToken).Coroutine();
            WaitSelectStorageSlot wait = await dialogueComponent.GetComponent<ObjectWait>().Wait<WaitSelectStorageSlot>();
            if (wait.index > 0)
            {
                //TODO 显示存档中UI
                DialogueStorageManager.Instance.QuickSaveShot.Save();
                DialogueStorageManager.Instance.OverWriteShot(wait.index, 0);
            }

            escapeToken.Cancel();
            self.ClientScene().GetComponent<UIComponent>().HideWindow<DlgStorage>();
            dialogueComponent.AddTag(DialogueTag.InDialogueCor);
        }

        private static async ETTask Load(this DlgDialogue self)
        {
            DialogueComponent dialogueComponent = TODUnitHelper.GetPlayer(self.ClientScene()).GetComponent<DialogueComponent>();
            if (!dialogueComponent.ContainTag(DialogueTag.CanEnterSetting)) return;

            dialogueComponent.RemoveTag(DialogueTag.InDialogueCor);
            await self.ClientScene().GetComponent<UIComponent>().ShowWindowAsync<DlgStorage>();
            self.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgStorage>().Refresh();

            ETCancellationToken escapToken = new();
            self.EscapeCor(escapToken).Coroutine();
            WaitSelectStorageSlot wait = await dialogueComponent.GetComponent<ObjectWait>().Wait<WaitSelectStorageSlot>();
            if (wait.index > 0 && !DialogueStorageManager.Instance.IsEmpty(wait.index))
            {
                //TODO 显示读取中UI
                DialogueStorageManager.Instance.OverWriteShot(0, wait.index);
                (uint treeID, uint targetID) = DialogueHelper.FromID(DialogueStorageManager.Instance.GetByIndex(wait.index).currentID);
                EventSystem.Instance.Invoke(new LoadTreeCallback()
                {
                    instanceId = dialogueComponent.InstanceId, 
                    ReloadType = ViewReloadType.Preview,
                    treeID = treeID,
                    targetID = targetID
                });
            }

            escapToken.Cancel();
            // self.ClientScene().GetComponent<UIComponent>().HideWindow<DlgStorage>(); //在根节点初始化中，token取消时会卸载UI,所以这里不要
            dialogueComponent.AddTag(DialogueTag.InDialogueCor);
        }

        //退出协程(注意这个不能热重载!!!)
        private static async ETTask EscapeCor(this DlgDialogue self, ETCancellationToken token)
        {
            while (true)
            {
                if (token.IsCancel()) return;
                //退出
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    DialogueComponent dialogueComponent = TODUnitHelper.GetPlayer(self.ClientScene()).GetComponent<DialogueComponent>();
                    dialogueComponent.GetComponent<ObjectWait>().Notify(new WaitSelectStorageSlot() { index = -1 });
                    return;
                }

                await TimerComponent.Instance.WaitFrameAsync();
            }
        }

        private static void Test(this DlgDialogue self)
        {
            Log.Warning(DialogueStorageManager.Instance.QuickSaveShot.ToString());
        }

        private static void CheckQS(this DlgDialogue self)
        {
            uint treeID = uint.Parse(self.View.E_CheckInput_TreeIDInputField.text);
            uint targetID = uint.Parse(self.View.E_CheckInput_TargetIDInputField.text);
            Log.Warning($"该节点是否已经执行?: " + DialogueStorageManager.Instance.QuickSaveShot.Check(treeID, targetID));
        }

        #region 选项

        public static void ShowChoicePanel(this DlgDialogue self, List<VN_ChoiceNode> nodes)
        {
            self.choiceNodes = nodes;
            self.View.E_ChoicePanelLoopVerticalScrollRect.RefreshCells();
            self.AddUIScrollItems(ref self.ScrollItemChoices, nodes.Count);
            self.View.E_ChoicePanelLoopVerticalScrollRect.SetVisible(true, nodes.Count);
        }

        public static void HideChoicePanel(this DlgDialogue self)
        {
            self.choiceNodes = null;
            self.RemoveUIScrollItems(ref self.ScrollItemChoices);
            self.View.E_ChoicePanelLoopVerticalScrollRect.SetVisible(false);
        }

        private static void OnLoopChoiceRefreshHandler(this DlgDialogue self, Transform transform, int index)
        {
            VN_ChoiceNode node = self.choiceNodes[index];
            Scroll_Item_Choice scrollItemChoice = self.ScrollItemChoices[index].BindTrans(transform);
            Unit player = TODUnitHelper.GetPlayer(self.ClientScene());
            DialogueComponent dialogueComponent = player.GetComponent<DialogueComponent>();

            var replaceText = node.text;
            DialogueHelper.ReplaceModel(player, ref replaceText);

            scrollItemChoice.E_SelectButton.onClick.RemoveAllListeners();
            scrollItemChoice.E_ContentText.SetText(replaceText);

            switch (node.choiceType)
            {
                default:
                    int ret = DialogueDispatcherComponent.Instance.Checks(player, node.handle_Configs);
                    //选项是否被锁定
                    scrollItemChoice.E_SelectImage.color = (ret == 0)? Color.white : Color.gray;
                    //选项是否可执行
                    if (ret == 0)
                    {
                        scrollItemChoice.E_SelectButton.AddListener(() =>
                        {
                            dialogueComponent.GetComponent<ObjectWait>().Notify(new WaitChoiceNode() { next = node.TargetID });
                        });
                    }
                    else
                    {
                        scrollItemChoice.E_SelectButton.onClick.RemoveAllListeners();
                    }

                    break;
            }
        }

        #endregion

        #region 箭头

        //初始化
        public static void RefreshArrow(this DlgDialogue self)
        {
            self.View.E_RightArrowButton.onClick.RemoveAllListeners();
            self.View.E_RightArrowButton.gameObject.SetActive(false);
            self.View.E_LeftArrowButton.onClick.RemoveAllListeners();
            self.View.E_LeftArrowButton.gameObject.SetActive(false);
        }

        public static void ShowRightArrow(this DlgDialogue self, Action action)
        {
            self.View.E_RightArrowButton.gameObject.SetActive(true);
            self.View.E_RightArrowButton.AddListener(action.Invoke);
        }

        public static void ShowLeftArrow(this DlgDialogue self, Action action)
        {
            self.View.E_LeftArrowButton.gameObject.SetActive(true);
            self.View.E_LeftArrowButton.AddListener(action.Invoke);
        }

        #endregion

        #region CharacterName

        public static void RefreshCharacterName(this DlgDialogue self, string text)
        {
            self.View.E_characterNameText.text = text;
        }

        #endregion
    }
}