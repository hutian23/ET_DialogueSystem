using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (DialogueComponent))]
    [FriendOf(typeof (DialogueDispatcherComponent))]
    public static class DialogueHelper
    {
        public static DialogueTreeData LoadDialogueTree(string treeName, Language language)
        {
            var file = Path.Combine(DialogueSettings.GetSettings().ExportPath, $"{treeName}.json");
            string jsonContent = File.ReadAllText(file);
            BsonDocument doc = MongoHelper.FromJson<BsonDocument>(jsonContent);
            var subDoc = doc["_v"].ToBsonDocument();

            return new DialogueTreeData(subDoc, language);
        }

        public static void ScripMatchError(string text)
        {
            Log.Error($"{text}匹配失败！请检查格式");
        }

        public static void Reload()
        {
            CodeLoader.Instance.LoadHotfix();
            EventSystem.Instance.Load();
            Log.Debug("hot reload success");
        }

        public static void ReplaceCustomModel(ref string text, string oldText, string newText)
        {
            string replaceStr = "{{" + oldText + "}}";
            text = text.Replace(replaceStr, newText);
        }

        public static string ReplaceModel(Unit unit, ref string replaceText)
        {
            if (string.IsNullOrEmpty(replaceText)) return string.Empty;
            MatchCollection matches = Regex.Matches(replaceText, @"<\w+\s+[^>]*\/>");

            foreach (Match match in matches)
            {
                string replaceType = match.Value.Split(' ')[0]; //<Numeric <UnitConfig
                replaceType = replaceType.Substring(1, replaceType.Length - 1);

                string replaceStr = DialogueDispatcherComponent.Instance.GetReplaceStr(unit, replaceType, match.Value);
                if (string.IsNullOrEmpty(replaceStr)) continue; //没找到对应的handler，不替换

                replaceText = replaceText.Replace(match.Value, replaceStr);
            }

            return replaceText;
        }

        #region DialogueDispatchComponent,避免和DialogueHelper产生环形依赖

        private static async ETTask CoroutineHandle(this DialogueDispatcherComponent self, Unit unit, DialogueNode node, List<string> corList,
        ETCancellationToken token)
        {
            int index = 0;
            while (index < corList.Count)
            {
                if (token.IsCancel())
                {
                    Log.Warning("canceld");
                    return;
                }

                var corLine = corList[index];
                if (string.IsNullOrEmpty(corLine) || corLine[0] == '#') // 空行 or 注释行 or 子命令
                {
                    index++;
                    continue;
                }

                var opLine = Regex.Split(corLine, @"- ")[1]; //把- 去掉，后面才是指令
                Match match = Regex.Match(opLine, @"^\w+");
                if (!match.Success)
                {
                    ScripMatchError(opLine);
                    return;
                }

                var opType = match.Value;
                var opCode = Regex.Match(opLine, "^(.*?);").Value; // ;后的不读取
                await self.ScriptHandle(unit, node, opType, opCode, token);
                if (token.IsCancel()) return;

                index++;
            }
        }

        /// <summary>
        /// 执行一行指令
        /// </summary>
        private static async ETTask ScriptHandle(this DialogueDispatcherComponent self, Unit unit, DialogueNode node, string opType, string opCode,
        ETCancellationToken token)
        {
            if (!self.scriptHandlers.TryGetValue(opType, out ScriptHandler handler))
            {
                Log.Error($"not found script handler: {opType}");
                return;
            }

            ReplaceModel(unit, ref opCode);
            await handler.Handle(unit, node, opCode, token);
        }

        public static async ETTask ScriptHandles(this DialogueDispatcherComponent self, Unit unit, DialogueNode node, ETCancellationToken token)
        {
            var opLines = node.Script.Split("\n"); // 一行一行执行
            int index = 0;

            while (index < opLines.Length)
            {
                var opLine = opLines[index];
                if (string.IsNullOrEmpty(opLine) || opLine[0] == '#' || opLine[0] == '-') // 空行 or 注释行 or 子命令
                {
                    index++;
                    continue;
                }

                if (opLine == "Coroutine:") // 携程行
                {
                    var corList = new List<string>();
                    while (++index < opLines.Length)
                    {
                        var coroutineLine = opLines[index];
                        if (string.IsNullOrEmpty(coroutineLine) || coroutineLine[0] == '#') continue;
                        if (coroutineLine[0] != '-') break;
                        corList.Add(coroutineLine);
                    }

                    self.CoroutineHandle(unit, node, corList, token).Coroutine();
                    continue;
                }

                Match match = Regex.Match(opLine, @"^\w+");
                if (!match.Success)
                {
                    ScripMatchError(opLine);
                    return;
                }

                var opType = match.Value;
                var opCode = Regex.Match(opLine, "^(.*?);").Value; // ;后的不读取

                await self.ScriptHandle(unit, node, opType, opCode, token);
                if (token.IsCancel()) return;

                index++;
            }
        }

        #endregion

        public static async ETTask WaitNextCor(ETCancellationToken token)
        {
            await TimerComponent.Instance.WaitAsync(200, token);
            if (token.IsCancel()) return;
            while (true)
            {
                if (token.IsCancel()) break;
                if (Keyboard.current.bKey.isPressed) return;
                await TimerComponent.Instance.WaitFrameAsync(token);
            }
        }

        #region DialogueComponent

        private static async ETTask SkipCheckCor(ETCancellationToken typeToken)
        {
            await TimerComponent.Instance.WaitAsync(200, typeToken);
            while (true)
            {
                if (typeToken.IsCancel()) break;
                if (Keyboard.current.bKey.isPressed)
                {
                    typeToken.Cancel();
                    return;
                }
                await TimerComponent.Instance.WaitFrameAsync(typeToken);
            }
        }

        [Invoke(TimerInvokeType.TypeingTimer)]
        public class TypeTimer: ATimer<DialogueComponent>
        {
            protected override void Run(DialogueComponent self)
            {
                self.RemoveTag(DialogueTag.Typing);
            }
        }

        public static async ETTask TypeCor(this DialogueComponent self, Text label, string content, ETCancellationToken token,
        bool CanSkip = true)
        {
            self.AddTag(DialogueTag.TypeCor); //标注一下当前在打字携程中

            ETCancellationToken typeToken = new(); //取消打印携程
            long timer = 0; //定时器，打字间隔时间过大，动画从talk-->idle
            token.Add(typeToken.Cancel);

            if (CanSkip) SkipCheckCor(typeToken).Coroutine();

            var currentText = "";
            var len = content.Length;
            var typeSpeed = Constants.TypeSpeed;
            var tagOpened = false;
            var tagType = ""; //标签属性

            for (int i = 0; i < len; i++)
            {
                // [#ts=300]
                if (content[i] == '[' && i + 4 < len && content.Substring(i, 5).Equals("[#ts="))
                {
                    string parseSpeed = "";
                    for (int j = i + 5; j < len; j++)
                    {
                        if (content[j] == ']')
                        {
                            break;
                        }

                        parseSpeed += content[j];
                    }

                    if (!int.TryParse(parseSpeed, out typeSpeed))
                    {
                        typeSpeed = Constants.TypeSpeed;
                    }

                    i += 6 + parseSpeed.Length - 1;
                    continue;
                }

                //[#wt=1000]
                if (content[i] == '[' && i + 4 < len && content.Substring(i, 5).Equals("[#wt="))
                {
                    string waitTimeStr = "";
                    int waitTime = 0;
                    for (int j = i + 5; j < len; j++)
                    {
                        if (content[j] == ']')
                        {
                            break;
                        }

                        waitTimeStr += content[j];
                    }

                    if (!int.TryParse(waitTimeStr, out waitTime))
                    {
                        Log.Error($"停顿时间转换失败:{waitTimeStr}");
                        return;
                    }
                    i += 6 + waitTimeStr.Length - 1;
                    
                    //快进了
                    if(!typeToken.IsCancel()) await TimerComponent.Instance.WaitAsync(waitTime, typeToken);
                    continue;
                }
                
                //ngui color tag(不知道是啥)
                if (content[i] == '[' && i + 7 < len && content[i + 7] == ']')
                {
                    currentText += content.Substring(i, 8);
                    i += 8 - 1;
                    continue;
                }

                bool symbolDetected = false;
                for (int j = 0; j < Constants._uguiSymbols.Length; j++)
                {
                    var symbol = $"<{Constants._uguiSymbols[j]}>";
                    if (content[i] == '<' && i + (1 + Constants._uguiSymbols[j].Length) < len
                        && content.Substring(i, 2 + Constants._uguiSymbols[j].Length).Equals(symbol))
                    {
                        currentText += symbol;
                        i += (2 + Constants._uguiSymbols[j].Length) - 1;
                        symbolDetected = true;
                        tagOpened = true;
                        tagType = Constants._uguiSymbols[j];
                        break;
                    }
                }

                //<color=#EA5B44FF></color>
                if (content[i] == '<' && i + (1 + 15) < len && content.Substring(i, 2 + 6).Equals("<color=#") && content[i + 16] == '>')
                {
                    currentText += content.Substring(i, 2 + 6 + 8);
                    i += (2 + 14) - 1;
                    symbolDetected = true;
                    tagOpened = true;
                    tagType = "color";
                }

                //<size=30></size>
                if (content[i] == '<' && i + 5 < len && content.Substring(i, 6).Equals("<size="))
                {
                    var parseSize = "";
                    for (var j = i + 6; j < len; j++)
                    {
                        if (content[j] == '>') break;
                        parseSize += content[j];
                    }

                    if (float.TryParse(parseSize, out float _))
                    {
                        currentText += content.Substring(i, 7 + parseSize.Length);
                        i += (parseSize.Length + 7) - 1;
                        symbolDetected = true;
                        tagOpened = true;
                        tagType = "size";
                    }
                }

                //exit symbol </color> </size>
                for (int j = 0; j < Constants._uguiCloseSymbols.Length; j++)
                {
                    var symbol = $"</{Constants._uguiCloseSymbols[j]}>";
                    if (content[i] == '<' && i + (2 + Constants._uguiCloseSymbols[j].Length) < len &&
                        content.Substring(i, 3 + Constants._uguiCloseSymbols[j].Length).Equals(symbol))
                    {
                        currentText += symbol;
                        i += (3 + Constants._uguiCloseSymbols[j].Length) - 1;
                        symbolDetected = true;
                        tagOpened = false;
                        break;
                    }
                }

                if (symbolDetected) continue;
                currentText += content[i];
                label.text = currentText + (tagOpened? $"</{tagType}>" : "");

                //这里这个token代表当前节点被取消执行了
                if (token.IsCancel()) return;
                if (typeToken.IsCancel()) continue;
                
                self.AddTag(DialogueTag.Typing);
                TimerComponent.Instance.Remove(ref timer);
                timer = TimerComponent.Instance.NewOnceTimer(TimeInfo.Instance.ClientNow() + 200, TimerInvokeType.TypeingTimer, self);
                await TimerComponent.Instance.WaitAsync(typeSpeed, typeToken);
            }

            TimerComponent.Instance.Remove(ref timer);
            self.RemoveTag(DialogueTag.TypeCor);
        }

        #endregion
    }
}