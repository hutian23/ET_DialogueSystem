using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class SceneChangeFinish_InitCurrentScene : AEvent<SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
            GameManager.Instance.Reload();
            await ETTask.CompletedTask;
        }
    }
}