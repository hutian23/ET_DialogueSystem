using Sirenix.Utilities;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (CharacterManager))]
    [FriendOf(typeof (Talker))]
    public static class CharacterManagerSystem
    {
        public class CharacterManagerAwakeSystem: AwakeSystem<CharacterManager>
        {
            protected override void Awake(CharacterManager self)
            {
                self.parent = new GameObject("CharacterManager");
                self.parent.transform.SetParent(GlobalComponent.Instance.Unit);
            }
        }

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
                UnityEngine.Object.Destroy(self.parent);
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

            self.talkers.Values.ForEach(self.RemoveChild);
            self.talkers.Clear();
        }

        public static async ETTask RegistCharacter(this CharacterManager self, string characterName, int unitId)
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
            go.transform.SetParent(self.parent.transform);

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

        public static Talker RegistTalker(this CharacterManager self, string characterName, string clip)
        {
            self.RemoveTalker(characterName);
            Talker talker = self.AddChild<Talker>();
            self.talkers.Add(characterName, talker.Id);
            talker.Init(characterName, clip);
            return talker;
        }

        public static void RemoveTalker(this CharacterManager self, string characterName)
        {
            if (self.talkers.TryGetValue(characterName, out long id))
            {
                self.RemoveChild(id);
                self.talkers.Remove(characterName);
            }
        }
    }
}