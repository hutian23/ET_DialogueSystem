using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (MarkerEventParser))]
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (DialogueDispatcherComponent))]
    public static class MarkerEventParserSystem
    {
        public class MarkerEventParserDestroySystem: DestroySystem<MarkerEventParser>
        {
            protected override void Destroy(MarkerEventParser self)
            {
                self.Cancel();
            }
        }

        public static void RegistMarker(this MarkerEventParser self, string markerName, int pointer)
        {
            if (self.marker_pointers.ContainsKey(markerName))
            {
                Log.Error($"Already exist MarkerEvent: {markerName}");
                return;
            }

            self.marker_pointers.Add(markerName, pointer);
        }

        public static bool ContainMarker(this MarkerEventParser self, string markerName)
        {
            return self.marker_pointers.ContainsKey(markerName);
        }

        private static void Cancel(this MarkerEventParser self)
        {
            self.marker_pointers.Clear();
            self.Token.Cancel();
        }

        public static void Invoke(this MarkerEventParser self, string markerName)
        {
            self.Token?.Cancel();
            self.Token = new ETCancellationToken();
            self.MarkerEventCoroutine(markerName).Coroutine();
        }

        private static async ETTask MarkerEventCoroutine(this MarkerEventParser self, string markerName)
        {
            //1. 动画帧事件入口指针
            if (!self.marker_pointers.TryGetValue(markerName, out int pointer))
            {
                Log.Error($"does not exist markerEvent:{markerName}");
                return;
            }

            //2. 动画帧事件相当于当前行为携程的一个子携程
            BBParser parser = self.GetParent<BBParser>();
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            parser.function_Pointers.Add(funcId, pointer);

            //3. 逐条执行语句
            parser.cancellationToken.Add(self.Token.Cancel);
            self.Token.Add(() => { parser.function_Pointers.Remove(funcId); }); // 执行完子携程，从协程列表中移除

            while (++parser.function_Pointers[funcId] < parser.opDict.Count)
            {
                if (self.Token.IsCancel()) break;

                //匹配opType
                string opLine = parser.opDict[parser.function_Pointers[funcId]];

                if (opLine.Equals("EndMarkerEvent:")) break;

                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    break;
                }

                string opType = match.Value;
                if (opType.Equals("SetMarker")) continue;

                //匹配handler
                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    break;
                }

                BBScriptData data = BBScriptData.Create(opLine, funcId, parser.currentID);
                Status ret = await handler.Handle(parser, data, self.Token);
                data.Recycle();

                if (self.Token.IsCancel() || ret is not Status.Success) break;
            }

            //4. 执行完子协程，从协程列表移除
            self.Token?.Cancel();

            await ETTask.CompletedTask;
        }
    }
}