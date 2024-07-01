namespace ET.Client
{
    public class ObjectionDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "Objection()";
        }

        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            var opLines = "HideWindow type = Dialogue;\nVN_RegistEffect name = objection prefabName = Objection;\nVN_Shake effect = objection curve = ShakeCurve duration = ShakeDuration intensity = ShakeIntensity;\nVN_RemoveEffect name = objection;\nWaitTime 500;";
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node, opLines, token);
        }
    }
}