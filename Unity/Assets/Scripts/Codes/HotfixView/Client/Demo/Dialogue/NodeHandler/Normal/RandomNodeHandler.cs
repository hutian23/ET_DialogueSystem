using System;
using System.Collections.Generic;

namespace ET.Client
{
    public class RandomNodeHandler : NodeHandler<RandomNode>
    {
        protected override async ETTask<Status> Run(Unit unit, RandomNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            // 检查前置条件
            var randomList = new List<uint>();
            node.random.ForEach(i =>
            {
                DialogueNode child = dialogueComponent.GetNode(i);
                if(child.NeedCheck && DialogueDispatcherComponent.Instance.Checks(unit,child.checkList) != 0) return;
                randomList.Add(i);
            });
            
            //符合条件的随机取一个
            int index = new Random().Next(0, randomList.Count);
            dialogueComponent.PushNextNode(randomList[index]);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}