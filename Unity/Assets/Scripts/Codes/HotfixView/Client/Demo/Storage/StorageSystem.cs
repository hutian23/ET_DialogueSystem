using System;
using System.IO;

namespace ET.Client
{
    [FriendOf(typeof(Storage))]
    public static class StorageSystem
    {
        public class StorageComponentAwakeSystem : AwakeSystem<Storage>
        {
            protected override void Awake(Storage self)
            {
                if (!Directory.Exists(self.path))
                {
                    Directory.CreateDirectory(self.path);
                }

                Storage.Instance = self;
            }
        }

        public static async ETTask<Unit> LoadStorage(this Storage self, int index)
        {
            var file = $"{self.path}/Storage{index + 1}.json";
            if (!File.Exists(file))
            {
                Log.Error($"不存在存档!: {file}");
                return null;
            }
            byte[] bytes = await File.ReadAllBytesAsync(file);
            return MongoHelper.Deserialize<Unit>(bytes);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public static async ETTask<bool> SaveStorage(this Storage self, int index, Unit player)
        {
            await using (FileStream save = File.Create($"{self.path}/Storage{index + 1}.json"))
            {
                try
                {
                    byte[] bytes = MongoHelper.Serialize(player);
                    save.Write(bytes);
                    Log.Debug($"保存路径为: {self.path}/Storage{index + 1}.json");
                    return true;
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    return false;
                }
            }
        }
        
    }
}