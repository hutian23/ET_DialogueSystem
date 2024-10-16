﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CallSubCoroutine_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CallSubCoroutine";
        }

        //CallSubCoroutine: 'OnBlock';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CallSubCoroutine func = (?<Function>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string funcName = match.Groups["Function"].Value;
            ETCancellationToken subToken = parser.RegistSubCoroutine(funcName);
            parser.Invoke(funcName, subToken).Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}