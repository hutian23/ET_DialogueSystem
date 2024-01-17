﻿using System.Text.RegularExpressions;

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

        public static void LogHappyNewYear()
        {
            Log.Warning("Happy new year");
        }
    }
}