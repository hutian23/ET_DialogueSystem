namespace ET.Client
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgFtg :Entity,IAwake,IUILogic,ILoad
	{

		public DlgFtgViewComponent View { get => this.GetComponent<DlgFtgViewComponent>();} 

		 

	}
}
