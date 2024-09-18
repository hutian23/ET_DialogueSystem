using ET.EventType;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class SceneChangeFinish_Createb2World: AEvent<SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
            scene.AddComponent<b2GameManager>();
            await ETTask.CompletedTask;
        }
    }
}