using System.Collections.Generic;

namespace ET.Client
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgStorage :Entity,IAwake,IUILogic,ILoad
	{
		public DlgStorageViewComponent View { get => this.GetComponent<DlgStorageViewComponent>();}

		public Dictionary<int, Scroll_Item_Storage> ScrollItemStorages = new();
	}
}
