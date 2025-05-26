namespace ET.Client
{
    [Invoke(EventType.CheckInput)]
    [FriendOf(typeof (InputWait))]
    public class CheckInputTimer: BBTimer<InputWait>
    {
        protected override void Run(InputWait self)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            
            //1. 缓存输入
            self.curOP = BBInputComponent.Instance.CheckInput();
            self.infoQueue.Enqueue(new InputInfo() { op = self.curOP, frame = sceneTimer.GetNow() });
            //超出容量的部分出列
            int count = self.infoQueue.Count;
            while (count-- > InputWait.MaxStack)
            {
                self.infoQueue.Dequeue();
            }

            //2. 更新输入历史
            self.HandleKeyInput(self.curOP, BBOperaType.X);
            self.HandleKeyInput(self.curOP, BBOperaType.A);
            self.HandleKeyInput(self.curOP, BBOperaType.Y);
            self.HandleKeyInput(self.curOP, BBOperaType.B);
            self.HandleKeyInput(self.curOP, BBOperaType.RB);
            self.HandleKeyInput(self.curOP, BBOperaType.RT);
            self.HandleKeyInput(self.curOP, BBOperaType.LB);
            self.HandleKeyInput(self.curOP, BBOperaType.LT);
            self.HandleKeyInput(self.curOP, BBOperaType.DOWNLEFT);
            self.HandleKeyInput(self.curOP, BBOperaType.LEFT);
            self.HandleKeyInput(self.curOP, BBOperaType.UPLEFT);
            self.HandleKeyInput(self.curOP, BBOperaType.UP);
            self.HandleKeyInput(self.curOP, BBOperaType.UPRIGHT);
            self.HandleKeyInput(self.curOP, BBOperaType.RIGHT);
            self.HandleKeyInput(self.curOP, BBOperaType.DOWNRIGHT);
            self.HandleKeyInput(self.curOP, BBOperaType.DOWN);
            self.HandleKeyInput(self.curOP, BBOperaType.MIDDLE);

            //3. 更新输入缓冲区
            count = self.infoQueue.Count;
            while (count-- > 0)
            {
                //找到对应的InputHandler
                string handlerName = self.handleQueue.Dequeue();
                self.handleQueue.Enqueue(handlerName);
                InputHandler handler = ScriptDispatcherComponent.Instance.GetInputHandler(handlerName);

                //更新缓冲最大有效帧
                string bufferType = handler.GetBufferType(); //缓冲类型
                long buffFrame = handler.Handle(self);
                if (!self.BufferDict.ContainsKey(bufferType))
                {
                    self.BufferDict.TryAdd(bufferType, -1);
                }
                
                if (buffFrame > self.BufferDict[bufferType])
                {
                    self.BufferDict[bufferType] = buffFrame;
                }
            }
        }
    }
}