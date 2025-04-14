namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorMachine))]    
    // 行为机模块对所有关联的模块进行初始化
    public class HandleBeforeBehaviorReloadCallback : AInvokeHandler<BeforeBehaviorReloadCallback>
    {
        public override void Handle(BeforeBehaviorReloadCallback args)
        {
            BehaviorMachine machine = Root.Instance.Get(args.instanceId) as BehaviorMachine;
            if (machine == null || machine.InstanceId == 0)
            {
                Log.Error($"cannot found behaviorMachine");
                return;
            }

            #region 切换行为时默认会执行的逻辑

            Unit unit = machine.GetParent<Unit>();
            BBParser parser = unit.GetComponent<BBParser>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();

            //1. 销毁当前行为协程执行中创建的共享变量
            parser.Cancel();
            foreach (var kv in machine.tmpParamDict)
            {
                parser.RegistParam(kv.Key, kv.Value.value);
            }
            machine.tmpParamDict.Clear();
            //2. 清空行为协程中生成的组件
            timelineComponent.Init();
            //3. 清空缓存的碰撞信息
            b2Unit.Init();

            #endregion

            #region 如果有拓展的需要，作为一个函数添加到脚本Root代码块中

            //Fin. 执行BeforeReload函数
            if (!parser.ContainFunction("Root", "BeforeReload")) return;
            parser.Invoke(parser.GetFunctionPointer("Root", "BeforeReload"), parser.CancellationToken).Coroutine();

            #endregion
        }
    }
}