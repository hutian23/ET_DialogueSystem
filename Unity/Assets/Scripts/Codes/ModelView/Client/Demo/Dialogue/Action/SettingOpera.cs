namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class SettingOpera: Entity, IAwake, IDestroy, IUpdate, ILoad
    {
        public long controllerTimer;
    }
}