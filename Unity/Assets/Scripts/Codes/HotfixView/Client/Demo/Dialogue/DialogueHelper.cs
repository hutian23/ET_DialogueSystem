﻿using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof (DialogueComponent))]
    public static class DialogueHelper
    {
        /// <summary>
        /// 替换{{model}}  (类似vue的插值表达式)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="modelName"></param>
        /// <param name="replaceText"></param>
        public static string Replace(string text, string modelName, string replaceText)
        {
            string pattern = @"\{\{" + modelName + @"\}\}";
            text = Regex.Replace(text, pattern, replaceText);
            return text;
        }

        public static void ScripMatchError(string text)
        {
            Log.Error($"{text}匹配失败！请检查格式");
        }

        public static async ETTask TypeCor(Text label, string content, ETCancellationToken token)
        {
            var currentText = "";
            var len = content.Length;
            var typeSpeed = Constants.TypeSpeed;
            var tagOpened = false; //标签内?
            var tagType = ""; //标签属性

            for (int i = 0; i < len; i++)
            {
                // [speed=300] --- 11
                if (content[i] == '[' && i + 6 < len && content.Substring(i, 7).Equals("[speed="))
                {
                    var parseSpeed = "";
                    for (var j = i + 7; j < len; j++)
                    {
                        if (content[j] == ']') break;
                        parseSpeed += content[j];
                    }

                    if (!int.TryParse(parseSpeed, out typeSpeed))
                    {
                        typeSpeed = Constants.TypeSpeed;
                    }

                    Log.Warning(parseSpeed + "  " + typeSpeed);
                    i += 8 + parseSpeed.Length - 1;
                    continue;
                }

                //ngui color tag(不知道是啥)
                if (content[i] == '[' && i + 7 < len && content[i + 7] == ']')
                {
                    currentText += content.Substring(i, 8);
                    i += 8 - 1;
                    continue;
                }

                var symbolDetected = false;
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
                await TimerComponent.Instance.WaitAsync(typeSpeed);
            }

            await ETTask.CompletedTask;
        }
    }
}