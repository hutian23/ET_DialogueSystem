namespace ET.Client
{
    public class VN_RegistEffect_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RegistEffect";
        }

        //VN_RegistEffect name = Hold_it prefabName = HoldIt;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            EffectManager manager = unit.GetComponent<DialogueComponent>().GetComponent<EffectManager>() == null
                    ? unit.GetComponent<DialogueComponent>().AddComponent<EffectManager>()
                    : unit.GetComponent<DialogueComponent>().GetComponent<EffectManager>();
            await ETTask.CompletedTask;
        }
    }
}