using UnityEngine;

namespace ET.Client
{
    public static class Scroll_Item_ChoiceSystem
    {
        public static void Refresh(this Scroll_Item_Choice self, VN_ChoiceNode node)
        {
            self.E_ContentText.SetText(node.text);

            Unit player = TODUnitHelper.GetPlayer(self.ClientScene());
            DialogueComponent dialogueComponent = player.GetComponent<DialogueComponent>();

            switch (node.choiceType)
            {
                case VN_ChoiceType.Vertification_Normal:
                    int ret = DialogueDispatcherComponent.Instance.Checks(player, node.handle_Config);
                    //选项是否被锁定
                    self.E_SelectImage.color = (ret == 0)? Color.white : Color.gray;
                    //选项是否可执行
                    if (ret == 0) self.E_SelectButton.AddListener(() =>
                    {
                        dialogueComponent.PushNextNode(node);
                        dialogueComponent.GetComponent<ObjectWait>().Notify(new WaitChoiceNode());
                    });
                    break;
            }
        }
    }
}