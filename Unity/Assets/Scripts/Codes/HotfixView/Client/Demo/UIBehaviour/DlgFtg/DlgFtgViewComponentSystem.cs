
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class DlgFtgViewComponentAwakeSystem : AwakeSystem<DlgFtgViewComponent> 
	{
		protected override void Awake(DlgFtgViewComponent self)
		{
			self.uiTransform = self.Parent.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgFtgViewComponentDestroySystem : DestroySystem<DlgFtgViewComponent> 
	{
		protected override void Destroy(DlgFtgViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
