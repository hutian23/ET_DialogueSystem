using Sirenix.Utilities;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (CharacterManager))]
    public static class CharacterManagerSystem
    {
        public class CharacterManagerLoadSystem: LoadSystem<CharacterManager>
        {
            protected override void Load(CharacterManager self)
            {
                self.Init();
            }
        }

        public class CharacterManagerDestroySystem: DestroySystem<CharacterManager>
        {
            protected override void Destroy(CharacterManager self)
            {
                self.Init();
            }
        }

        private static void Init(this CharacterManager self)
        {
            self.characters.Values.ForEach(id =>
            {
                UnitComponent unitComponent = self.ClientScene().CurrentScene().GetComponent<UnitComponent>();
                unitComponent.RemoveChild(id);
            });
            self.characters.Clear();
        }

        public static async ETTask RegisterCharacter(this CharacterManager self, string characterName, int unitId)
        {
            if (self.characters.ContainsKey(characterName))
            {
                Log.Error($"存在同名角色{characterName}");
                return;
            }
            UnitConfig config = UnitConfigCategory.Instance.Get(unitId);

            await ResourcesComponent.Instance.LoadBundleAsync($"{config.ABName}.unity3d");
            GameObject prefab = ResourcesComponent.Instance.GetAsset($"{config.ABName}.unity3d", config.Name) as GameObject;
            GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
            go.name = $"{characterName}";
            //注意 角色都是在currentScene下的
            UnitComponent unitComponent = self.ClientScene().CurrentScene().GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChild<Unit, int>(config.Id);
            unit.AddComponent<GameObjectComponent>().GameObject = go;

            unit.AddComponent<AnimatorComponent>();
            self.characters.Add(characterName, unit.Id);
        }

        public static void RemoveCharacter(this CharacterManager self, string characterName)
        {
            if (self.characters.TryGetValue(characterName, out long unitId))
            {
                UnitComponent unitComponent = self.ClientScene().CurrentScene().GetComponent<UnitComponent>();
                unitComponent.RemoveChild(unitId);
                self.characters.Remove(characterName);
            }
        }

        public static Unit GetCharacter(this CharacterManager self, string characterName)
        {
            if (self.characters.TryGetValue(characterName, out long unitId))
            {
                return self.ClientScene().CurrentScene().GetComponent<UnitComponent>().Get(unitId);
            }

            Log.Error($"不存在角色{characterName}");
            return null;
        }
    }
}