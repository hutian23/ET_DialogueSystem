
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class DlgTestViewComponentAwakeSystem : AwakeSystem<DlgTestViewComponent> 
	{
		protected override void Awake(DlgTestViewComponent self)
		{
			self.uiTransform = self.Parent.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgTestViewComponentDestroySystem : DestroySystem<DlgTestViewComponent> 
	{
		protected override void Destroy(DlgTestViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
