namespace ET.Client
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgTest :Entity,IAwake,IUILogic,ILoad
	{

		public DlgTestViewComponent View { get => this.GetComponent<DlgTestViewComponent>();} 

		 

	}
}




