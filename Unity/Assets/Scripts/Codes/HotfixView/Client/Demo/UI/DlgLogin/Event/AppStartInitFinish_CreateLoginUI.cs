using ET.EventType;

namespace ET.Client
{
	[Event(SceneType.Client)]
	public class AppStartInitFinish_CreateLoginUI: AEvent<AppStartInitFinish>
	{
		protected override async ETTask Run(Scene scene, AppStartInitFinish args)
		{
			// scene.GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Login);
			await scene.GetComponent<UIComponent>().ShowWindowAsync<DlgTest>();
			await ETTask.CompletedTask;
		}
	}
}
