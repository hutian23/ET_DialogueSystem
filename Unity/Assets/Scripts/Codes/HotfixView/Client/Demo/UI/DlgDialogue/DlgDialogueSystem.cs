using System;
using System.Collections.Generic;
using UnityEngine;

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
            self.View.E_QuickSaveButton.AddListener(self.Save);
            self.View.E_ChoicePanelLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopChoiceRefreshHandler);
        }

        public class DlgDialogueLoadSystem: LoadSystem<DlgDialogue>
        {
            protected override void Load(DlgDialogue self)
            {
                self.View.E_ClearQSButton.AddListener(self.ClearQuickSave);
                self.View.E_CheckQuickSaveButton.AddListener(self.CheckQS);
                self.View.E_ChoicePanelLoopVerticalScrollRect.AddItemRefreshListener(self.OnLoopChoiceRefreshHandler);
                self.RefreshArrow();
            }
        }

        public static void ShowWindow(this DlgDialogue self, Entity contextData = null)
        {
            self.RefreshArrow();
        }

        private static void ClearQuickSave(this DlgDialogue self)
        {
            Log.Warning("清空缓存!!!");
            DialogueStorageManager.Instance.QuickSaveShot.ClearBuffer();
            DialogueStorageManager.Instance.QuickSaveShot.storageSet.Clear();
        }

        private static void Save(this DlgDialogue self)
        {
            DialogueStorageManager.Instance.QuickSaveShot.Save();
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
                case VN_ChoiceType.Vertification_Normal:
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
    }
}