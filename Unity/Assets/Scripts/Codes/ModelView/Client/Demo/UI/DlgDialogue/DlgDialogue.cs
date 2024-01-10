namespace ET.Client
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgDialogue :Entity,IAwake,IUILogic,ILoad
	{

		public DlgDialogueViewComponent View { get => this.GetComponent<DlgDialogueViewComponent>();} 

		 

	}
}
