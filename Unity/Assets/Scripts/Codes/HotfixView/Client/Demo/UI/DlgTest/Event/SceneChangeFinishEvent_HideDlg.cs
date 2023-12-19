using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeFinishEvent_HideDlg : AEvent<SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish a)
        {
            scene.GetComponent<UIComponent>().HideWindow<DlgTest>();
            await ETTask.CompletedTask;
        }
    }
}