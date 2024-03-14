using System.Text.RegularExpressions;

namespace ET.Client
{
    public class AddGatlingOperation_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AddGatlingOperation";
        }

        //AddGatlingCancel: 'Sol_5HS';
        //之前参照b站up主刘笑寒的视频,指令有缓冲之外，当执行一个行为时，其他行为也会被缓冲，当可以GC的时候取出来，判断满足前置条件吗，满足，则执行这个行为
        //例如 JP 2626HP 满足SA1和重风神的指令条件，两个行为都被缓冲了.如果有必杀槽，可以释放SA1; 如果没有，则变成了重风神
        //
        //我的思路:
        //1. 不关闭输入检测协程，每帧检测有无符合条件的行为，有，则缓冲这个行为
        //2. 栈 缓冲行为(后进先出) 行为缓冲数据结构中需要包含缓冲行为的帧号，顺序(例如，SA比必杀顺序高，相同必杀中，PO的指令投顺序比其他必杀高)
        //3. 可GC取消时(设置一个窗口)，每帧遍历栈，找到符合条件的行为(包括数值检测等等),执行
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "AddGatlingOperation: '(?<skill>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string skillValue = match.Groups["skill"].Value;
            parser.GetParent<DialogueComponent>().GetComponent<GatlingCancel>().AddTag(skillValue);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}