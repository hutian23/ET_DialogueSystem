using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (EmojiComponent))]
    public static class EmojiComponentSystem
    {
        public class EmojiComponentDestroySystem: DestroySystem<EmojiComponent>
        {
            protected override void Destroy(EmojiComponent self)
            {
                GameObjectPoolHelper.ReturnObjectToPool(self.emoji);
            }
        }

        public class EmojiComponentLoadSystem: LoadSystem<EmojiComponent>
        {
            protected override void Load(EmojiComponent self)
            {
                self.Dispose();
            }
        }

        public static async ETTask SpawnEmoji(this EmojiComponent self, string emojiType, ETCancellationToken token)
        {
            string prefabName = $"Emoji_{emojiType}";
            if (!GameObjectPoolHelper.ContainPool(prefabName))
            {
                await ResourcesComponent.Instance.LoadBundleAsync($"{prefabName}.unity3d");
                GameObject prefab = ResourcesComponent.Instance.GetAsset($"{prefabName}.unity3d", prefabName) as GameObject;
                await GameObjectPoolHelper.InitPoolFormGamObjectAsync(prefab, 3);
            }

            if (token.IsCancel()) return;

            Unit unit = self.GetParent<Unit>();
            self.emoji = GameObjectPoolHelper.GetObjectFromPool(prefabName);

            switch (emojiType)
            {
                case "Chaos":
                    Transform chaosSpawnDefaultPoint = unit.GetRC<Transform>("ChaosSpawnDefaultPoint");
                    self.emoji.transform.SetParent(chaosSpawnDefaultPoint);
                    self.emoji.transform.localPosition = Vector3.zero;
                    break;
            }

            await ETTask.CompletedTask;
        }
    }
}