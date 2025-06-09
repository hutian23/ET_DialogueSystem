namespace ET.Client
{
    [FriendOf(typeof(EnhanceInputComponent))]
    public static class EnhanceInputSystem
    {
        public class EnhanceInputAwakeSystem : AwakeSystem<EnhanceInputComponent>
        {
            protected override void Awake(EnhanceInputComponent self)
            {
                self.enhanceDict.Clear();
            }
        }
        
        [FriendOf(typeof(InputComponent))]
        public class EnhanceInputFrameUpdateSystem : FrameUpdateSystem<EnhanceInputComponent>
        {
            protected override void FrameUpdate(EnhanceInputComponent self)
            {
                InputComponent input = self.GetParent<BBParser>().GetParent<Unit>().GetComponent<InputComponent>();
                BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
                
                // 作为外挂组件，操作inputComponent中的数据
                int count = input.infoQueue.Count;
                while (count -- > 0)
                {
                    string handlerName = input.handleQueue.Dequeue();
                    input.handleQueue.Enqueue(handlerName);
                    
                    // 该输入不需要增强
                    InputHandler handler = ScriptDispatcherComponent.Instance.GetInputHandler(handlerName);
                    string bufferType = handler.GetBufferType();
                    if(!self.enhanceDict.TryGetValue(bufferType, out int _buffFrame)) continue;
                    
                    // 判定失败
                    long buffFrame = handler.Handle(input);
                    if (buffFrame <= 0) continue;
            
                    // 增强输入
                    if (_buffFrame + sceneTimer.GetNow() > input.BufferDict[bufferType])
                    {
                        input.BufferDict[bufferType] = _buffFrame + sceneTimer.GetNow();
                    }
                }
            }
        }

        public class EnhanceInputDestroySystem : DestroySystem<EnhanceInputComponent>
        {
            protected override void Destroy(EnhanceInputComponent self)
            {
                self.enhanceDict.Clear();
            }
        }

        public static void RegistEnhanceInput(this EnhanceInputComponent self, string inputType, int buffFrame)
        {
            self.enhanceDict.TryAdd(inputType, buffFrame);
        }
    }
}