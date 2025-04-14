using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Random_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Random";
        }

        //Random: ran, 0, 10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Random: (?<Random>\w+), (?<min>.*?), (?<max>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["min"].Value, out int min) || !int.TryParse(match.Groups["max"].Value, out int max))
            {
                Log.Error($"cannot format {match.Groups["min"].Value} / {match.Groups["max"].Value} to int!!!");
                return Status.Failed;
            }
            
            string paramName = $"Random_{match.Groups["Random"].Value}";
            
            //注册变量
            parser.TryRemoveParam(paramName);
            int value = new Random().Next(min, max + 1);
            parser.RegistParam(paramName, value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}