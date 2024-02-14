namespace ET.Client
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgStorage :Entity,IAwake,IUILogic,ILoad
	{

		public DlgStorageViewComponent View { get => this.GetComponent<DlgStorageViewComponent>();} 

		 

	}
}
