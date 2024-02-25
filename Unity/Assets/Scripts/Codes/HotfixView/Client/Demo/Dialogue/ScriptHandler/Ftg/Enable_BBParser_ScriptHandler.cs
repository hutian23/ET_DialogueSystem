namespace ET.Client
{
    public class Enable_BBParser_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "Enable_BBParser";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            unit.GetComponent<DialogueComponent>().AddComponent<BBParser>();
            await ETTask.CompletedTask;
        }
    }
}