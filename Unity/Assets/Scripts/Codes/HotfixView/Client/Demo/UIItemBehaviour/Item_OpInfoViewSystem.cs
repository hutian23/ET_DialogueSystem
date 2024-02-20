
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class Scroll_Item_OPInfoDestroySystem : DestroySystem<Scroll_Item_OPInfo> 
	{
		protected override void Destroy( Scroll_Item_OPInfo self )
		{
			self.DestroyWidget();
		}
	}
}
