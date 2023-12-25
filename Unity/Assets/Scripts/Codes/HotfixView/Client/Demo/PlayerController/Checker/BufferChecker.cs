namespace ET.Client
{
    public class BufferChecker: CheckerHandler
    {
        public override int Execute(Unit player, CheckerConfig config)
        {
            ChainComponent chain = player.GetComponent<Skill_InfoComponent>().GetComponent<ChainComponent>();

            int punchBufferTime = config.GetInt("PunchBufferTime");
            int slashBufferTime = config.GetInt("SlashBufferTime");
            
            if (Input.Instance.CheckInput(OperaType.PunchWasPressed))
            {
                PunchBuffer punchBuffer = chain.GetComponent<PunchBuffer>() == null? chain.GetComponent<PunchBuffer>()
                        : chain.AddComponent<PunchBuffer>();
                punchBuffer.ResetTimer(punchBufferTime);
            }

            if (Input.Instance.CheckInput(OperaType.SlashWasPressed))
            {
                SlashBuffer slashBuffer = chain.GetComponent<SlashBuffer>() == null? chain.GetComponent<SlashBuffer>()
                        : chain.AddComponent<SlashBuffer>();
            }
            
            return 0;
        }
    }
}