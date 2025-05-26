namespace ET.Client
{
	[Event(SceneType.Client)]
	public class LoginFinish_RemoveLoginUI: AEvent<ET.EventType.LoginFinish>
	{
		protected override async ETTask Run(Scene scene, ET.EventType.LoginFinish args)
		{
			scene.GetComponent<UIComponent>().HideWindow(WindowID.WindowID_Login);
			await ETTask.CompletedTask;
		}
	}
}
