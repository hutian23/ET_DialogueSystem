using UnityEngine.InputSystem;

namespace ET.Client
{
    [Invoke(EventType.ThronesTestTimer)]
    [FriendOf(typeof(BBParser))]
    public class ThronesTestTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            //检测按键输入
            Keyboard _key = Keyboard.current;
            bool ret = _key.aKey.isPressed || _key.sKey.isPressed || _key.dKey.isPressed;
            if (!ret) return;
            
            //初始化
            self.CancellationToken.Cancel();
            self.Coroutine_Pointers.Clear();
            self.CancellationToken = new();
            
            //回调
            int startIndex = self.GetParam<int>($"ThronesTest_StartIndex");
            int endIndex = self.GetParam<int>($"ThronesTest_EndIndex");
            self.RegistSubCoroutine(startIndex, endIndex, self.CancellationToken).Coroutine();
        }
    }
    
    [FriendOf(typeof(BBParser))]
    public class ThronesTest_Timer_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ThronesTest";
        }

        //ThronesTest:
        //EndThronesTest:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndThronesTest:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;
            
            // 初始化
            parser.TryRemoveParam("ThronesTest_StartIndex");
            parser.TryRemoveParam("ThronesTest_EndIndex");
            BBTimerComponent bbTimer = BBTimerManager.Instance.SceneTimer();
            if (parser.ContainParam("ThronesTestTimer"))
            {
                long preTimer = parser.GetParam<long>("ThronesTestTimer");
                bbTimer.Remove(ref preTimer);
            }
            parser.TryRemoveParam("ThronesTestTimer");
            
            // 注册变量
            parser.RegistParam($"ThronesTest_StartIndex", startIndex);
            parser.RegistParam($"ThronesTest_EndIndex", endIndex);
            long timer = bbTimer.NewFrameTimer(EventType.ThronesTestTimer, parser);
            parser.RegistParam($"ThronesTestTimer", timer);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}