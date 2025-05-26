using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(InjectorComponent))]
    [FriendOf(typeof(ScriptDispatcherComponent))]
    public static class InjectionComponentSystem
    {
        public class InjectionComponentDestroySystem : DestroySystem<InjectorComponent>
        {
            protected override void Destroy(InjectorComponent self)
            {
                self.Init();
            }
        }

        private static void Init(this InjectorComponent self)
        {
            self.OpDict.Clear();
            foreach (var kv in self.GroupDict)
            {
                kv.Value.Recycle();
            }
            self.GroupDict.Clear();
            self.GroupPointerSet.Clear();
            self.token?.Cancel();
            self.Coroutine_Pointers.Clear();
            self.token = new ETCancellationToken();
        }

        public static async ETTask<Status> Invoke(this InjectorComponent self, int index, ETCancellationToken token)
        {
            BBParser parser = self.GetParent<Unit>().GetComponent<BBParser>();
            
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            self.Coroutine_Pointers.Add(funcId, index);

            Status ret = Status.Success;
            while (++self.Coroutine_Pointers[funcId] < self.OpDict.Count)
            {
                string opLine = self.OpDict[self.Coroutine_Pointers[funcId]];
                if (self.GroupPointerSet.Contains(self.Coroutine_Pointers[funcId]))
                {
                    ret = Status.Failed;
                    break;
                }

                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    ret = Status.Failed;
                    break;
                }

                string opType = match.Value;
                if (!ScriptDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    ret = Status.Failed;
                    break;
                }
                
                BBScriptData data = BBScriptData.Create(opLine, funcId);
                ret = await handler.Handle(parser, data, token);
                data.Recycle();

                if (token.IsCancel() || ret != Status.Success) break;
            }

            self.Coroutine_Pointers.Remove(funcId);
            return ret;
        }

        public static bool ContainGroup(this InjectorComponent self, string groupName)
        {
            return self.GroupDict.ContainsKey(groupName);
        }

        public static int GetGroupPointer(this InjectorComponent self, string groupName)
        {
            if (!self.GroupDict.TryGetValue(groupName, out DataGroup group))
            {
                Log.Error($"not found dataGroup: {groupName}");
                return -1;
            }
            return group.startIndex;
        }

        public static bool ContainFunction(this InjectorComponent self, string groupName, string funcName)
        {
            if (!self.GroupDict.TryGetValue(groupName, out DataGroup group))
            {
                return false;
            }
            return group.funcPointers.ContainsKey(funcName);
        }

        public static int GetFunctionPointer(this InjectorComponent self, string groupName, string funcName)
        {
            if (!self.GroupDict.TryGetValue(groupName, out DataGroup group))
            {
                Log.Error($"not found dataGroup: {groupName}");
                return -1;
            }
            if (!group.funcPointers.TryGetValue(funcName, out int funcPointer))
            {
                Log.Error($"not found funcPointer: {funcPointer}");
                return -1;
            }
            return funcPointer;
        }

        public static int GetMarkerPointer(this InjectorComponent self, string groupName, string markerName)
        {
            if (!self.GroupDict.TryGetValue(groupName, out DataGroup group))
            {
                Log.Error($"not found dataGroup: {groupName}");
                return -1;
            }
            if (!group.markerPointers.TryGetValue(markerName, out int markerPointer))
            {
                Log.Error($"not found markerPointer: {markerPointer}");
                return -1;
            }
            return markerPointer;
        }
    }
}