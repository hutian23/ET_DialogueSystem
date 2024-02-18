
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class Scroll_Item_OpInfoDestroySystem : DestroySystem<Scroll_Item_OpInfo> 
	{
		protected override void Destroy( Scroll_Item_OpInfo self )
		{
			self.DestroyWidget();
		}
	}
}
