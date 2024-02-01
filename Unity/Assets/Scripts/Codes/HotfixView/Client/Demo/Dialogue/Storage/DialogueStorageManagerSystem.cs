namespace ET.Client
{
    [FriendOf(typeof (DialogueStorageManager))]
    public static class DialogueStorageManagerSystem
    {
        public class DialogueStorageManagerAwakeSystem: AwakeSystem<DialogueStorageManager>
        {
            protected override void Awake(DialogueStorageManager self)
            {
                DialogueStorageManager.Instance = self;

                self.shots = new long[DialogueStorageManager.MaxSize];
                for (int i = 0; i < DialogueStorageManager.MaxSize; i++)
                {
                    DialogueStorage storage = self.AddChild<DialogueStorage>();
                    self.shots[i] = storage.Id;
                }
            }
        }

        public class DialogueStorageManagerDeserializeSystem: DeserializeSystem<DialogueStorageManager>
        {
            protected override void Deserialize(DialogueStorageManager self)
            {
                DialogueStorageManager.Instance = self;
            }
        }

        public static int GetShotIndex(this DialogueStorageManager self, DialogueStorage storage)
        {
            for (int i = 0; i < self.shots.Length; i++)
            {
                if (storage.Id == self.shots[i])
                {
                    return i;
                }
            }

            return -1;
        }

        public static DialogueStorage GetByIndex(this DialogueStorageManager self, int index)
        {
            return self.GetChild<DialogueStorage>(self.shots[index]);
        }

        /// <summary>
        /// 移除存档组件(以及挂载在组件上的其他实体)
        /// </summary>
        public static void ClearShot(this DialogueStorageManager self, int index)
        {
            DialogueStorage oldStorage = self.GetChild<DialogueStorage>(self.shots[index]);
            self.RemoveChild(oldStorage.Id);
            
            DialogueStorage newStorage = self.AddChild<DialogueStorage>();
            self.shots[index] = newStorage.Id;
        }

        /// <summary>
        /// 覆盖存档
        /// </summary>
        public static void OverWriteShot(this DialogueStorageManager self, int sourceIndex, int overWriteIndex)
        {
            //源存档被覆盖
            DialogueStorage sourceStorage = self.GetChild<DialogueStorage>(self.shots[sourceIndex]);
            self.RemoveChild(sourceStorage.Id);
            
            DialogueStorage overWriteStorage = self.GetChild<DialogueStorage>(self.shots[overWriteIndex]);
            DialogueStorage cloneStorage = MongoHelper.Clone(overWriteStorage);
            self.AddChild(cloneStorage);
            self.shots[sourceIndex] = cloneStorage.Id;
        }
    }
}