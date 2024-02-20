
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class Scroll_Item_OPDestroySystem : DestroySystem<Scroll_Item_OP> 
	{
		protected override void Destroy( Scroll_Item_OP self )
		{
			self.DestroyWidget();
		}
	}
}
