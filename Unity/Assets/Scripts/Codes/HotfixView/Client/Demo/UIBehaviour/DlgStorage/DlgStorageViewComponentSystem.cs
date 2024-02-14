
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class DlgStorageViewComponentAwakeSystem : AwakeSystem<DlgStorageViewComponent> 
	{
		protected override void Awake(DlgStorageViewComponent self)
		{
			self.uiTransform = self.Parent.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgStorageViewComponentDestroySystem : DestroySystem<DlgStorageViewComponent> 
	{
		protected override void Destroy(DlgStorageViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
