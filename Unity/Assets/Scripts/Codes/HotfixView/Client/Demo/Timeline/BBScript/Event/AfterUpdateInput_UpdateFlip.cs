namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof (b2Body))]
    [FriendOf(typeof (InputWait))]
    public class AfterUpdateInput_UpdateFlip: AEvent<AfterUpdateInput>
    {
        protected override async ETTask Run(Scene scene, AfterUpdateInput args)
        {
            InputWait inputWait = Root.Instance.Get(args.instanceId) as InputWait;

            //Update press dict 
            for (int i = 0; i < 16; i++)
            {
                Log.Warning(i+"  "+(2 << i));
            }

            await ETTask.CompletedTask;
        }
    }
}