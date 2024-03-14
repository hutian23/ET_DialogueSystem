namespace ET.Client
{
    public class BBNodeHandler: NodeHandler<BBNode>
    {
        protected override async ETTask<Status> Run(Unit unit, BBNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();

            //移除所有加特林取消
            dialogueComponent.GetComponent<GatlingCancel>().Init();
            //清除回调
            ObjectWait objectWait = dialogueComponent.GetComponent<ObjectWait>();
            objectWait.Notify(new WaitBlock() { Error = WaitTypeError.Destroy });
            objectWait.Notify(new WaitHit() { Error = WaitTypeError.Destroy });
            objectWait.Notify(new WaitCounterHit() { Error = WaitTypeError.Destroy });

            dialogueComponent.GetComponent<BBParser>().InitScript(node);
            return await dialogueComponent.GetComponent<BBParser>().Main(token);
        }
    }
}