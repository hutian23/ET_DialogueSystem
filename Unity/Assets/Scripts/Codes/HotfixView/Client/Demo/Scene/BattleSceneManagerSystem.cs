using System.Collections.Generic;
using System.Linq;

namespace ET.Client
{
    public static class BattleSceneManagerSystem
    {
        public class BattleSceneManagerAwakeSystem : AwakeSystem<BattleSceneManager>
        {
            protected override void Awake(BattleSceneManager self)
            {
                BattleSceneManager.Instance = self;
            }
        }
        
        public class BattleSceneManagerLoadSystem : LoadSystem<BattleSceneManager>
        {
            protected override void Load(BattleSceneManager self)
            {
                List<Entity> removeList = ListComponent<Entity>.Create();
                removeList.AddRange(self.Children.Values.Where(child => child is Unit));
                removeList.ForEach(entity => entity.Dispose());
            }
        }
        
        public class BattleSceneManagerDestroySystem : DestroySystem<BattleSceneManager>
        {
            protected override void Destroy(BattleSceneManager self)
            {
                BattleSceneManager.Instance = null;
            }
        }
    }
}