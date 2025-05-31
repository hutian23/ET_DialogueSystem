
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class ESCommonUIAwakeSystem : AwakeSystem<ESCommonUI,Transform> 
	{
		protected override void Awake(ESCommonUI self,Transform filterType)
		{
			self.uiTransform = filterType;
		}
	}


	[ObjectSystem]
	public class ESCommonUIDestroySystem : DestroySystem<ESCommonUI> 
	{
		protected override void Destroy(ESCommonUI self)
		{
			self.DestroyWidget();
		}
	}
}
