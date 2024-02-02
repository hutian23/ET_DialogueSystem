
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ObjectSystem]
	public class Scroll_Item_ChoiceDestroySystem : DestroySystem<Scroll_Item_Choice> 
	{
		protected override void Destroy( Scroll_Item_Choice self )
		{
			self.DestroyWidget();
		}
	}
}
