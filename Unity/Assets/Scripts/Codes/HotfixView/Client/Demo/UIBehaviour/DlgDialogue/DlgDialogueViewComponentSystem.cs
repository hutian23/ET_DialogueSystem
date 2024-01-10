
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class DlgDialogueViewComponentAwakeSystem : AwakeSystem<DlgDialogueViewComponent> 
	{
		protected override void Awake(DlgDialogueViewComponent self)
		{
			self.uiTransform = self.Parent.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgDialogueViewComponentDestroySystem : DestroySystem<DlgDialogueViewComponent> 
	{
		protected override void Destroy(DlgDialogueViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
