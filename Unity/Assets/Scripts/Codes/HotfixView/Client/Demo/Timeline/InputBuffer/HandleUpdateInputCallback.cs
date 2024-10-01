namespace ET.Client
{
    [Event(SceneType.Current)]
    public class HandleUpdateInputCallback: AEvent<UpdateInputCallback>
    {
        protected override async ETTask Run(Scene scene, UpdateInputCallback a)
        {
            await ETTask.CompletedTask;
        }
    }
}