namespace ET.Client
{
    //https://www.zhihu.com/question/36951135/answer/69880133
    [FriendOf(typeof (InputComponent))]
    public static class InputComponentSystem
    {
        public class InputComponentAwakeSystem : AwakeSystem<InputComponent>
        {
            protected override void Awake(InputComponent self)
            {
                self.Init();
            }
        }

        public class InputComponentFrameUpdateSystem : FrameUpdateSystem<InputComponent>
        {
            protected override void FrameUpdate(InputComponent self)
            {
                BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();

                //1. 缓存输入
                self.curOP = BBInputManager.Instance.CheckInput();
                self.infoQueue.Enqueue(new InputInfo() { op = self.curOP, frame = sceneTimer.GetNow() });
                //超出容量的部分出列
                int count = self.infoQueue.Count;
                while (count-- > InputComponent.MaxStack)
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
        
        private static void Init(this InputComponent self)
        {
            self.curOP = 0;
            self.infoQueue.Clear();
            self.handleQueue.Clear();
            self.BufferDict.Clear();
            self.PressedDict.Clear();
            self.IsPressingDict.Clear();
            self.PressingDict.Clear();
            self.RegistKeyHistory();
        }

        #region KeyHistory

        private static void RegistKeyHistory(this InputComponent self)
        {
            self.PressedDict.Add(BBOperaType.X, -1);
            self.PressingDict.Add(BBOperaType.X, -1);
            self.IsPressingDict.Add(BBOperaType.X, false);
            
            self.PressedDict.Add(BBOperaType.A, -1);
            self.PressingDict.Add(BBOperaType.A, -1);
            self.IsPressingDict.Add(BBOperaType.A, false);

            self.PressedDict.Add(BBOperaType.Y, -1);
            self.PressingDict.Add(BBOperaType.Y, -1);
            self.IsPressingDict.Add(BBOperaType.Y, false);
            
            self.PressedDict.Add(BBOperaType.B, -1);
            self.PressingDict.Add(BBOperaType.B, -1);
            self.IsPressingDict.Add(BBOperaType.B, false);
            
            self.PressedDict.Add(BBOperaType.RT, -1);
            self.PressingDict.Add(BBOperaType.RT, -1);
            self.IsPressingDict.Add(BBOperaType.RT, false);
            
            self.PressedDict.Add(BBOperaType.RB, -1);
            self.PressingDict.Add(BBOperaType.RB, -1);
            self.IsPressingDict.Add(BBOperaType.RB, false);
            
            self.PressedDict.Add(BBOperaType.LB, -1);
            self.PressingDict.Add(BBOperaType.LB, -1);
            self.IsPressingDict.Add(BBOperaType.LB, false);
            
            self.PressedDict.Add(BBOperaType.LT, -1);
            self.PressingDict.Add(BBOperaType.LT, -1);
            self.IsPressingDict.Add(BBOperaType.LT, false);

            self.PressedDict.Add(BBOperaType.DOWNLEFT, -1);
            self.PressingDict.Add(BBOperaType.DOWNLEFT, -1);
            self.IsPressingDict.Add(BBOperaType.DOWNLEFT, false);

            self.PressedDict.Add(BBOperaType.LEFT, -1);
            self.PressingDict.Add(BBOperaType.LEFT, -1);
            self.IsPressingDict.Add(BBOperaType.LEFT, false);
            
            self.PressedDict.Add(BBOperaType.UPLEFT, -1);
            self.PressingDict.Add(BBOperaType.UPLEFT, -1);
            self.IsPressingDict.Add(BBOperaType.UPLEFT, false);
            
            self.PressedDict.Add(BBOperaType.UP, -1);
            self.PressingDict.Add(BBOperaType.UP, -1);
            self.IsPressingDict.Add(BBOperaType.UP, false);
            
            self.PressedDict.Add(BBOperaType.UPRIGHT, -1);
            self.PressingDict.Add(BBOperaType.UPRIGHT, -1);
            self.IsPressingDict.Add(BBOperaType.UPRIGHT, false);
            
            self.PressedDict.Add(BBOperaType.RIGHT, -1);
            self.PressingDict.Add(BBOperaType.RIGHT, -1);
            self.IsPressingDict.Add(BBOperaType.RIGHT, false);
            
            self.PressedDict.Add(BBOperaType.DOWNRIGHT, -1);
            self.PressingDict.Add(BBOperaType.DOWNRIGHT, -1);
            self.IsPressingDict.Add(BBOperaType.DOWNRIGHT, false);
            
            self.PressedDict.Add(BBOperaType.DOWN, -1);
            self.PressingDict.Add(BBOperaType.DOWN, -1);
            self.IsPressingDict.Add(BBOperaType.DOWN, false);
            
            self.PressedDict.Add(BBOperaType.MIDDLE, -1);
            self.PressingDict.Add(BBOperaType.MIDDLE, -1);
            self.IsPressingDict.Add(BBOperaType.MIDDLE, false);
        }
        
        public static bool WasPressedThisFrame(this InputComponent self, long operaType)
        {
            return self.PressedDict[operaType] == BBTimerManager.Instance.SceneTimer().GetNow();
        }

        public static bool WasReleasedThisFrame(this InputComponent self, long operaType)
        {
            return self.PressingDict[operaType] >= 0 && BBTimerManager.Instance.SceneTimer().GetNow() - self.PressingDict[operaType] == 1;
        }
        
        public static bool IsPressed(this InputComponent self, int operaType)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            return self.PressingDict[operaType] == sceneTimer.GetNow();
        }

        public static bool IsPressing(this InputComponent self, int operaType)
        {
            return self.IsPressingDict[operaType];
        }
        
        public static bool IsReleased(this InputComponent self, int operaType)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            return self.PressingDict[operaType] < sceneTimer.GetNow();
        }

        public static long GetPressedFrame(this InputComponent self, int operaType)
        {
            return self.PressedDict[operaType];
        }
        
        /// <summary>
        /// 比如5帧之前按下x键之后松开，设置有效帧数为6帧，则此时仍然可以判定x为按下状态
        /// </summary>
        /// <param name="self"></param>
        /// <param name="operaType">要查询的指令</param>
        /// <param name="buffFrame">在给定帧数内认为按键仍然有效</param>
        /// <returns></returns>
        public static bool IsKeyCached(this InputComponent self, int operaType, int buffFrame = 5)
        {
            if (self.PressingDict[operaType] == -1)
            {
                return false;
            }

            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            return self.PressingDict[operaType] + buffFrame >= sceneTimer.GetNow();
        }
        
        #endregion

        private static void HandleKeyInput(this InputComponent self, long ops, int operaType)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            bool ret = (ops & operaType) != 0;
            if (ret)
            {
                //按键持续按住
                self.PressingDict[operaType] = sceneTimer.GetNow();
                //记录按键哪一帧按下
                if (!self.IsPressingDict[operaType])
                {
                    self.PressedDict[operaType] = sceneTimer.GetNow();
                }
                //按键处于按住状态
                self.IsPressingDict[operaType] = true;
            }
            else
            {
                self.IsPressingDict[operaType] = false;
            }
        }
        
        public static bool ContainKey(this InputComponent self, long op)
        {
            return (self.curOP & op) != 0;
        }

        public static bool CheckBuffer(this InputComponent self, string bufferName, long curFrame)
        {
            if (!self.BufferDict.TryGetValue(bufferName, out long timeOutFrame))
            {
                return false;
            }
            return timeOutFrame >= curFrame;
        }
    }
}