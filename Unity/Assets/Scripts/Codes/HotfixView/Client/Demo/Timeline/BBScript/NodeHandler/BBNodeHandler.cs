﻿using Timeline;

namespace ET.Client
{
    public class BBNodeHandler: NodeHandler<BBNode>
    {
        protected override async ETTask<Status> Run(Unit unit, BBNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            BehaviorBufferComponent bufferComponent = dialogueComponent.GetComponent<BehaviorBufferComponent>();
            // TimelineManager timelineManager = dialogueComponent.GetComponent<TimelineManager>();
            // RootMotionComponent rootMotion = dialogueComponent.GetComponent<RootMotionComponent>();
            //
            // //移除所有加特林取消
            // bufferComponent.ClearWhiff();
            // bufferComponent.ClearGC();
            //
            // //初始化timeline
            // long skillOrder = bufferComponent.GetOrder(node.TargetID);
            // timelineManager.Init(skillOrder);
            //
            // //enable root motion
            // rootMotion.Init(node.TargetID);
            //
            // dialogueComponent.GetComponent<BBParser>().InitScript(node);
            // return await dialogueComponent.GetComponent<BBParser>().Main();
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}