using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (DialogueStorage))]
    public static class DialogueStorageSystem
    {
        public class DialogueStorageDestroySystem: DestroySystem<DialogueStorage>
        {
            protected override void Destroy(DialogueStorage self)
            {
                self.storageSet.Clear();
                self.nodeIDTemp.Clear();
            }
        }

        public class DialogueStorageDeserializeSystem: DeserializeSystem<DialogueStorage>
        {
            protected override void Deserialize(DialogueStorage self)
            {
                self.currentID_Temp = self.currentID;
            }
        }

        #region 增

        public static bool Add(this DialogueStorage self, long ID)
        {
            return self.storageSet.Add(ID);
        }

        public static bool Add(this DialogueStorage self, DialogueNode node)
        {
            return self.storageSet.Add(node.GetID());
        }

        #endregion

        #region 删

        private static bool Remove(this DialogueStorage self, long ID)
        {
            return self.storageSet.Remove(ID);
        }

        public static bool Remove(this DialogueStorage self, DialogueNode node)
        {
            long ID = node.GetID();
            return self.Remove(ID);
        }

        /// <summary>
        /// 移除该对话树的全部历史记录
        /// </summary>
        public static void RemoveTree(this DialogueStorage self, uint treeID)
        {
            foreach (long ID in self.storageSet)
            {
                (uint _treeID, uint _) = FromID(ID);
                if (treeID != _treeID) continue;
                self.Remove(ID);
            }
        }

        #endregion

        #region 改

        //没有,哈哈

        #endregion

        #region 查

        public static bool Check(this DialogueStorage self, long ID)
        {
            return self.storageSet.Contains(ID) || self.nodeIDTemp.Contains(ID);
        }

        public static bool Check(this DialogueStorage self, DialogueNode node)
        {
            long ID = node.GetID();
            return self.Check(ID);
        }

        public static bool Check(this DialogueStorage self, uint treeID, uint targetID)
        {
            return self.Check(ToID(treeID, targetID));
        }

        /// <summary>
        /// 返回一颗树中所有的保存的节点
        /// </summary>
        /// <returns>targetID列表</returns>
        public static List<uint> CheckTree(this DialogueStorage self, uint treeID)
        {
            var caches = new List<uint>();
            foreach (long ID in self.storageSet)
            {
                (uint _treeID, uint targetID) = FromID(ID);
                if (_treeID != treeID) continue;
                caches.Add(targetID);
            }

            return caches;
        }

        #endregion

        private static (uint, uint) FromID(long ID)
        {
            ulong result = (ulong)ID;
            uint targetID = (uint)(result & uint.MaxValue);
            result >>= 32;
            uint treeID = (uint)(result & uint.MaxValue);
            return (treeID, targetID);
        }

        private static long ToID(uint treeID, uint targetID)
        {
            ulong result = 0;
            result |= targetID;
            result |= (ulong)treeID << 32;
            return (long)result;
        }

        #region 缓冲

        public static void AddToBuffer(this DialogueStorage self, uint treeID, uint targetID)
        {
            ulong ID = 0;
            ID |= targetID;
            ID |= (ulong)treeID << 32;
            self.nodeIDTemp.Add((long)ID);
        }

        public static void AddToBuffer(this DialogueStorage self, DialogueNode node)
        {
            long ID = node.GetID();
            self.nodeIDTemp.Add(ID);
        }

        public static void AddToBuffer(this DialogueStorage self, long ID)
        {
            self.nodeIDTemp.Add(ID);
            self.currentID_Temp = ID;
        }

        public static void ClearBuffer(this DialogueStorage self)
        {
            self.nodeIDTemp.Clear();
            self.currentID_Temp = self.currentID;
        }

        /// <summary>
        /// 清空缓冲区的id，全部保存
        /// </summary>
        /// <param name="self"></param>
        public static void Save(this DialogueStorage self)
        {
            self.nodeIDTemp.ForEach(ID => { self.storageSet.Add(ID); });
            self.nodeIDTemp.Clear();
            self.currentID = self.currentID_Temp;
        }

        #endregion
    }
}