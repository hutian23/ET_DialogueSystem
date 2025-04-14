namespace ET.Client
{
    [FriendOf(typeof(AirCheckComponent))]
    public static class AirCheckComponentSystem
    {
        // [Invoke(BBTimerInvokeType.AirCheckTimer)]
        // [FriendOf(typeof(B2Unit))]
        // [FriendOf(typeof(AirCheckComponent))]
        // public class AirCheckTimer : BBTimer<Unit>
        // {
        //     protected override void Run(Unit self)
        //     {
        //         //1. 查询组件 
        //         B2Unit b2Unit = self.GetComponent<B2Unit>();
        //         AirCheckComponent airCheck = self.GetComponent<AirCheckComponent>();
        //         
        //         //2. 从碰撞缓冲区中取出碰撞信息，逐个检测
        //         Queue<CollisionInfo> infoQueue = b2Unit.TriggerBuffer;
        //         int count = infoQueue.Count;
        //         while (count-- > 0)
        //         {
        //             CollisionInfo info = infoQueue.Dequeue();
        //             infoQueue.Enqueue(info);
        //
        //             if (info.dataA.Name.Equals("AirCheckBox") && info.dataB.LayerMask is LayerType.Ground && !info.dataB.IsTrigger)
        //             {
        //                 if (airCheck.inAir)
        //                 {
        //                     EventSystem.Instance.Invoke(new LandCallback(){instanceId = self.InstanceId});
        //                 }
        //                 airCheck.inAir = false;
        //             }
        //         }
        //         
        //         airCheck.inAir = true;   
        //     }
        // }

        public class AirCheckComponentAwakeSystem : AwakeSystem<AirCheckComponent>
        {
            protected override void Awake(AirCheckComponent self)
            {
            }
        }

        public class AirCheckComponentDestroySystem : DestroySystem<AirCheckComponent>
        {
            protected override void Destroy(AirCheckComponent self)
            {
            }
        }

        public static bool GetInAir(this AirCheckComponent self)
        {
            return self.inAir;
        }
    }
}