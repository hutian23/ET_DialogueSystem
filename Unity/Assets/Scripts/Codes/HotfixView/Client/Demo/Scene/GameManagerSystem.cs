using Testbed.Abstractions;

namespace ET.Client
{
    public static class GameManagerSystem
    {
        public class GameManagerAwakesystem: AwakeSystem<GameManager>
        {
            protected override void Awake(GameManager self)
            {
                GameManager.Instance = self;
            }
        }
        
        public class GameManagerLoadSystem : LoadSystem<GameManager>
        {
            protected override void Load(GameManager self)
            {
                self.Reload();
            }
        }

        public static void Reload(this GameManager self)
        {
            //Editor
            Global.Settings.Pause = false;
            
            //2. b2World reload
            b2GameManager.Instance.Reload();
            //1. timeline
            TimelineManager.Instance.Reload();
        }
    }
}