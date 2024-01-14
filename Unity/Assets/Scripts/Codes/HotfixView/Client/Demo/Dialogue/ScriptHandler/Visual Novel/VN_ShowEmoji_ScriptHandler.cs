namespace ET.Client
{
    public class VN_ShowEmoji_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_ShowEmoji";
        }

        public override async ETTask Handle(Unit unit, string line, ETCancellationToken token)
        {
            unit.RemoveComponent<EmojiComponent>();
            await unit.AddComponent<EmojiComponent>().SpawnEmoji("Chaos", token);
        }
    }
}