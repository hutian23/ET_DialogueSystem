using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Client
{
    [ChildOf(typeof (DialogueStorageManager))]
    public class DialogueStorage: Entity, IAwake, IDestroy, IDeserialize, ISerializeToEntity
    {
        // key treeID 32bit + targetID 32bits
        public HashSet<ulong> storageSet = new();

        //缓存的对话
        [BsonIgnore]
        public List<ulong> nodeIDTemp = new();
    }
}