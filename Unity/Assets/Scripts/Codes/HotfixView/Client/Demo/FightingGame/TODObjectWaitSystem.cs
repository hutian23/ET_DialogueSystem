using System;
using System.Linq;

namespace ET.Client
{
    [FriendOfAttribute(typeof (ET.Client.TODObjectWait))]
    public static class TODObjectWaitSystem
    {
        public class TODObjectWaitAwakeSystem: AwakeSystem<TODObjectWait>
        {
            protected override void Awake(TODObjectWait self)
            {
                self.tcss.Clear();
            }
        }

        public class TODObjectWaitLoadSystem: LoadSystem<TODObjectWait>
        {
            protected override void Load(TODObjectWait self)
            {
                foreach (object v in self.tcss.Values.ToArray())
                {
                }

                self.tcss.Clear();
            }
        }

        public static async ETTask<T> Wait<T>(this TODObjectWait self, ETCancellationToken token = null) where T : struct, IWaitType
        {
            ResultCallback<T> tcs = new ResultCallback<T>();
            Type type = typeof (T);
            self.tcss.Add(type, tcs);

            void CancelAction()
            {
                
            }

            return default;
        }

        public static void Notify<T>(this TODObjectWait self, T obj) where T : struct, IWaitType
        {
            Type type = typeof (T);
            if (!self.tcss.TryGetValue(type, out object tcss))
            {
                return;
            }

            self.tcss.Remove(type);
            
        }
    }
}