using ET.EventType;

namespace ET.Client
{
    [NumericWatcher(SceneType.Client, NumericType.Hp)]
    public class HpWatcher: INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            Log.Warning($"血量改变,{args.New}");
        }
    }
}