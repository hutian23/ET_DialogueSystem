using Sirenix.Utilities;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (EffectManager))]
    public static class EffectManagerSystem
    {
        public class EffectManagerAwakeSystem: AwakeSystem<EffectManager>
        {
            protected override void Awake(EffectManager self)
            {
                self.parent = new GameObject("EffectManager");
                self.parent.transform.SetParent(GlobalComponent.Instance.Unit);
            }
        }

        public class EffectManagerDestroySystem: DestroySystem<EffectManager>
        {
            protected override void Destroy(EffectManager self)
            {
                self.dic.Values.ForEach(i => { UnityEngine.Object.Destroy(i); });
                self.dic.Clear();
                UnityEngine.Object.Destroy(self.parent);
            }
        }

        public static async ETTask RegistEffect(this EffectManager self, string name, string prefabName)
        {
            if (self.dic.ContainsKey(name))
            {
                Log.Error($"存在同名特效:{name}");
                return;
            }

            await ResourcesComponent.Instance.LoadBundleAsync($"{prefabName.ToLower()}.unity3d");
            GameObject prefab = ResourcesComponent.Instance.GetAsset($"{prefabName.ToLower()}.unity3d", prefabName) as GameObject;
            GameObject go = UnityEngine.Object.Instantiate(prefab, self.parent.transform, true);
            go.name = $"{name}";
            self.dic.Add(name, go);
        }

        public static GameObject GetEffect(this EffectManager self, string name)
        {
            if (!self.dic.TryGetValue(name, out GameObject go))
            {
                Log.Error($"不存在特效: {name}");
                return null;
            }
            return go;
        }
    }
}