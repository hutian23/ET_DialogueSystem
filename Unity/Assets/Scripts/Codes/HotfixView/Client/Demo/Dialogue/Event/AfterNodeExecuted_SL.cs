namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AfterNodeExecuted_SL: AEvent<AfterNodeExecuted>
    {
        protected override async ETTask Run(Scene scene, AfterNodeExecuted args)
        {
            DialogueStorageManager.Instance.QuickSaveShot.AddToBuffer(args.ID);
            await ETTask.CompletedTask;
        }
    }
}