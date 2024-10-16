﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorBufferComponent))]
    [FriendOf(typeof (BBParser))]
    public class WhiffWindow_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WhiffWindow";
        }

        //WhiffWindow: 10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"WhiffWindow: (?<whiffTag>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //窗口持续帧数
            int.TryParse(match.Groups["whiffTag"].Value, out int lastedFrame);

            WhiffCancelCor(parser, lastedFrame).Coroutine();
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask WhiffCancelCor(BBParser parser, int lastedFrame)
        {
            DialogueComponent dialogueComponent = parser.GetParent<DialogueComponent>();
            BBTimerComponent bbTimer = dialogueComponent.GetComponent<BBTimerComponent>();
            BehaviorBufferComponent bufferComponent = dialogueComponent.GetComponent<BehaviorBufferComponent>();

            List<long> orderSet = bufferComponent.WhiffSet.OrderByDescending(order => order).ToList();
            
            //1.注册为一个子协程
            ETCancellationToken whiffToken = parser.RegistSubCoroutine("WhiffWindow");
            
            int count = 0;
            while (count++ < lastedFrame)
            {
                await bbTimer.WaitFrameAsync(whiffToken);
                if (whiffToken.IsCancel()) return;

                foreach (long order in orderSet.Where(order => bufferComponent.OrderSet.Contains(order)))
                {
                    parser.Cancel();
                    dialogueComponent.GetComponent<ObjectWait>().Notify(new WaitNextBehavior() { order = order });
                    break;
                }
            }
        }
    }
}