﻿using System.Linq;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorBufferComponent))]
    [FriendOf(typeof(BBParser))]
    public class CancelImmediately_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CancelImmediatly";
        }

        //CancelImmediately;
        //每帧检测buffer，取出优先级最高的(必须优先级比当前行为高)
        //eg. Idle行为中，可以在任意一帧中取消到其他行为,因为Idle是优先级最低的(Normal,0)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            CancelImmediatelyCor(parser, data).Coroutine();
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask CancelImmediatelyCor(BBParser parser, BBScriptData data)
        {
            DialogueComponent dialogueComponent = parser.GetParent<DialogueComponent>();
            BehaviorBufferComponent bufferComponent = dialogueComponent.GetComponent<BehaviorBufferComponent>();
            BBTimerComponent bbTimer = dialogueComponent.GetComponent<BBInputComponent>().GetComponent<BBTimerComponent>();

            long currentOrder = bufferComponent.behaviorDict[data.targetID].GetOrder();

            ETCancellationToken token = parser.RegistSubCoroutine("CancelImmediately");
            
            //每帧检测
            while (true)
            {
                //取出优先级最高的当前帧可执行行为(如果为相同行为，不切换)
                var orderSet = bufferComponent.OrderSet.OrderByDescending(order => order);
                foreach (long order in orderSet)
                {
                    //同一行为，不切换
                    if (order == currentOrder) continue;
                    parser.Cancel();
                    dialogueComponent.GetComponent<ObjectWait>().Notify(new WaitNextBehavior() { order = order });
                    return;
                }

                if (token.IsCancel()) return;
                await bbTimer.WaitFrameAsync(token);
            }
        }
    }
}