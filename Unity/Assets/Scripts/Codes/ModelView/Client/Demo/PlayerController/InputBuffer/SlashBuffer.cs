namespace ET.Client
{
    [ComponentOf(typeof (ChainComponent))]
    public class SlashBuffer: Entity, IAwake, IDestroy
    {
        public long bufferTimer;

        public int moveX;
    }
}