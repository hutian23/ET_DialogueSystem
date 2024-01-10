using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;

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

        #region 增

        public static bool Add(this DialogueStorage self, DialogueNode dialogueNode)
        {
            ulong ID = self.GetStorageID(dialogueNode);
            return self.storageSet.Add(ID);
        }

        public static void AddTree(this DialogueStorage self, DialogueTree tree)
        {
            tree.targets.Values.ForEach(node => self.Add(node));
        }

        #endregion

        #region 删

        public static bool Remove(this DialogueStorage self, DialogueNode dialogueNode)
        {
            ulong ID = self.GetStorageID(dialogueNode);
            return self.storageSet.Remove(ID);
        }

        /// <summary>
        /// 移除该对话树的全部历史记录
        /// </summary>
        public static void RemoveTree(this DialogueStorage self, DialogueTree tree)
        {
            tree.targets.Values.ForEach(node => self.Remove(node));
        }

        #endregion

        #region 改

        //没有

        #endregion

        #region 查

        public static bool Check(this DialogueStorage self, DialogueNode node)
        {
            ulong ID = self.GetStorageID(node);
            return self.storageSet.Contains(ID);
        }

        public static bool Check(this DialogueStorage self, uint treeID, uint targetID)
        {
            ulong ID = 0;
            ID |= (ulong)treeID << 32;
            ID |= targetID;
            return self.storageSet.Contains(ID);
        }

        //返回一颗树中所有的保存的节点
        public static List<uint> Check(this DialogueStorage self, DialogueTree tree)
        {
            var caches = new List<uint>();
            self.storageSet.ForEach(ID =>
            {
                ulong result = ID;
                uint targetID = (uint)result & uint.MaxValue;
                result >>= 32;
                uint treeID = (uint)result & uint.MaxValue;
                if (treeID == tree.treeID) caches.Add(targetID);
            });
            return caches;
        }

        public static List<DialogueNode> GetStorageNode(this DialogueStorage self, DialogueTree tree)
        {
            var storages = self.Check(tree);
            return tree.targets.Values.Where(node => storages.Contains(node.TargetID)).ToList();
        }

        #endregion

        /// <summary>
        ///  StorageID = treeID + targetID
        /// </summary>
        private static ulong GetStorageID(this DialogueStorage self, DialogueNode node)
        {
            ulong ID = 0;
            ID |= (ulong)node.TreeID << 32;
            ID |= node.TargetID;
            return ID;
        }

        #region 缓冲

        public static void AddToBuffer(this DialogueStorage self, uint treeID, uint targetID)
        {
            ulong ID = 0;
            ID |= (ulong)treeID << 32;
            ID |= targetID;
            self.nodeIDTemp.Add(ID);
        }

        public static void ClearBuffer(this DialogueStorage self)
        {
            self.nodeIDTemp.Clear();
        }

        /// <summary>
        /// 清空缓冲区的id，全部保存
        /// </summary>
        /// <param name="self"></param>
        public static void Save(this DialogueStorage self)
        {
            self.nodeIDTemp.ForEach(ID => { self.storageSet.Add(ID); });
            self.nodeIDTemp.Clear();
        }

        #endregion
    }
}