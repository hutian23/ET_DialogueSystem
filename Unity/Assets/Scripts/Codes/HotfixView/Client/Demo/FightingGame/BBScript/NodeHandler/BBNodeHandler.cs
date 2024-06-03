using Timeline;

namespace ET.Client
{
    public class BBNodeHandler: NodeHandler<BBNode>
    {
        protected override async ETTask<Status> Run(Unit unit, BBNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            BehaviorBufferComponent bufferComponent = dialogueComponent.GetComponent<BehaviorBufferComponent>();
            //移除所有加特林取消
            bufferComponent.ClearWhiff();
            bufferComponent.ClearGC();

            //清除回调
            // ObjectWait objectWait = dialogueComponent.GetComponent<ObjectWait>();
            // objectWait.Notify(new WaitBlock() { Error = WaitTypeError.Destroy });
            // objectWait.Notify(new WaitHit() { Error = WaitTypeError.Destroy });
            // objectWait.Notify(new WaitCounterHit() { Error = WaitTypeError.Destroy });

            TimelinePlayer timelinePlayer = unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<TimelinePlayer>();
            long skillOrder = bufferComponent.GetOrder(node.TargetID);
            timelinePlayer.Init(timelinePlayer.GetByOrder((int)skillOrder));
            Log.Warning(node.TargetID + "  " + skillOrder);

            dialogueComponent.GetComponent<BBParser>().InitScript(node);
            return await dialogueComponent.GetComponent<BBParser>().Main();
        }
    }
}