using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeFinishEvent_CreateDialogueHelp : AEvent<SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish arg)
        {
            await scene.GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Dialogue);
        }
    }
}