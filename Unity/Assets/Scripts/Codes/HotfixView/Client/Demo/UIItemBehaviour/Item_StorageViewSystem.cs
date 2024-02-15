
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class Scroll_Item_StorageDestroySystem : DestroySystem<Scroll_Item_Storage> 
	{
		protected override void Destroy( Scroll_Item_Storage self )
		{
			self.DestroyWidget();
		}
	}
}
