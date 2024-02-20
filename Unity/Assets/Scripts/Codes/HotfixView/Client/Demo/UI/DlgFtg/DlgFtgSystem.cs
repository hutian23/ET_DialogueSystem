using UnityEngine;

namespace ET.Client
{
	[FriendOf(typeof(DlgFtg))]
	public static  class DlgFtgSystem
	{

		public static void RegisterUIEvent(this DlgFtg self)
		{
		}

		public static void ShowWindow(this DlgFtg self, Entity contextData = null)
		{
		}


		public static void Refresh(this DlgFtg self,long ops)
		{
			// self.View.E_Arrow_DownImage.color;
		}

		private static void ChangeTransparency(this DlgFtg self, float alpha = 150f)
		{
			
		}
	}
}
