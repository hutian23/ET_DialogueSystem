﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (ScriptParser))]
    public class StartCoroutine_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "StartCoroutine";
        }

        //StartCoroutine: 'Test1';
        public override async ETTask<Status> Handle(Unit unit, ScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "StartCoroutine: '(?<FunctionName>.*?)','(?<CoroutineName>.*?)';");
            if (!match.Success)
            {
                ScriptHelper.ScriptMatchError(data.opLine);
                return Status.Failed;
            }

            ScriptParser parser = unit.GetComponent<TimelineComponent>().GetComponent<ScriptParser>();
            parser.CallSubCoroutine(match.Groups["FunctionName"].Value, match.Groups["CoroutineName"].Value).Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}