namespace ET.Client
{
    public class VN_RegistTalkCor_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RegisterTalkCor";
        }

        //VN_RegistTalkCor ch = Skye clip = Happy_talk;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            
        }
    }
}