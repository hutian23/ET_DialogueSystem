using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class HotReloadManager : Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static HotReloadManager Instance;
        public Queue<BBScript> BBScriptQueue = new();
    }

    // 挂载HotReloadManager时调用
    public struct HotReloadInitCallback
    {
    }
    
    public struct HotReloadCallBack
    {
        
    }
}