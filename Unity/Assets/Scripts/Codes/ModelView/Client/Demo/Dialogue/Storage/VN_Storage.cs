namespace ET.Client
{
    [ComponentOf(typeof(DialogueStorage))]
    public class VN_Storage : Entity,IAwake,IDestroy,ISerializeToEntity
    {
        public long test_ID;
    }
}