namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    public class HandleBehaviorReloadCallback : AInvokeHandler<BehaviorReloadCallback>
    {
        public override void Handle(BehaviorReloadCallback args)
        {
            //1. 获取组件
            Unit unit = Root.Instance.Get(args.unitId) as Unit;
            BehaviorInfo info = Root.Instance.Get(args.infoId) as BehaviorInfo;
            BBParser bbParser = unit.GetComponent<BBParser>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            
            //2. 取消当前的行为协程
            machine.SetCurrentOrder(info.behaviorOrder);
            bbParser.Cancel();

            #region MyRegion
            Transition transition = unit.GetComponent<Transition>();
            transition?.CacheFlag();
            
            if (bbParser.ContainFunction("Root", "BeforeReloadCallback"))
            {
                bbParser.Invoke(bbParser.GetFunctionPointer("Root", "BeforeReloadCallback"), bbParser.CancellationToken).Coroutine();
            }
            
            #endregion 行为切换前的逻辑

            //4. 行为切换
            bbParser.Invoke(bbParser.GetFunctionPointer(info.behaviorName, "Main"), bbParser.CancellationToken).Coroutine();
            
            //5. 切换行为后回调
            if (bbParser.ContainFunction("Root", "AfterReloadCallback"))
            {
                bbParser.Invoke(bbParser.GetFunctionPointer("Root", "BeforeReloadCallback"), bbParser.CancellationToken).Coroutine();
            }
        }
    }
}