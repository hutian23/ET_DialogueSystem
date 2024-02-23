namespace ET.Client
{
    [ObjectSystem]
    public class Hadoken_OPHandler
    {
        //必杀前置条件检测 + 按键检测
        // public async ETTask<Status> CheckHandler(Unit unit, ETCancellationToken token)
        // {
        // TODWait todWait = unit.GetComponent<TODWait>();
        // //现在假设这是一个OD波动拳释放检测协程
        // //第一段
        // InputBuffer state_1 = await todWait.Wait(
        //     op: TODOperaType.DOWNLEFT | TODOperaType.DOWN, //模糊输入
        //     cancellationToken: token, //用来取消等待协程,被取消了后面的都不执行了(if cancel state_1.Error  == WaitTypeError.Destroy)
        //     waitType: 0,   // 0表示以上任意一个都可以判定成成功, 1表示OD即需要以上任意两个按下
        //     waitFrame: int.MaxValue); // 犹豫区间,超出这个时间，结束当前协程，启动一个新的检测协程
        // if (state_1.Error != WaitTypeError.Success) return Status.Failed;
        //
        // //第二段
        // InputBuffer status_2 = await todWait.Wait(op: TODOperaType.DOWNRIGHT, cancellationToken: token, waitType: 0, waitFrame: 5);
        // if (status_2.Error != WaitTypeError.Success) return Status.Failed;
        //
        // //第三段
        // InputBuffer status_3 = await todWait.Wait(op: TODOperaType.RIGHT | TODOperaType.UPRIGHT, cancellationToken: token, waitType: 0, waitFrame: 5);
        // if (status_3.Error != WaitTypeError.Success) return Status.Failed;
        //
        // //等待按下 LP + MP / LP + HP / MP + HP
        // InputBuffer status_4 = await todWait.Wait(op: TODOperaType.LIGHTPUNCH | TODOperaType.MIDDLEPUNCH | TODOperaType.HEAVYPUNCH, cancellationToken: token, waitType: 1, waitFrame: 5);
        // if (status_4.Error != WaitTypeError.Success) return Status.Failed;
        //
        // //气不够
        // if (unit.GetComponent<NumericComponent>()[NumericType.SP] <= 20) return Status.Failed;
        //
        // //如果是JP的OD地刺的情况，LP+MP 放出来的和 LP+HP 的不同
        // if ((status_4.inputInfo & (TODOperaType.LIGHTPUNCH | TODOperaType.MIDDLEPUNCH)) != 0) { }
        // else if ((status_4.inputInfo & (TODOperaType.LIGHTPUNCH | TODOperaType.HEAVYPUNCH)) != 0) { }
        // else { }
        //
        // //取消其他必杀检测协程(不会清空缓冲区输入)
        // token.Cancel();
        // return Status.Success;
        // }
    }
}