using System.Collections.Generic;

namespace ET.Client
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgDialogue :Entity,IAwake,IUILogic,ILoad
	{
		public DlgDialogueViewComponent View { get => this.GetComponent<DlgDialogueViewComponent>();}

		public Dictionary<int, Scroll_Item_Choice> ScrollItemChoices = new();

		public List<VN_ChoiceNode> choiceNodes = new();
	}
}
