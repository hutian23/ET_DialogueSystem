using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Client
{
    //Unit ---> BBNumeric
    //Unit ---> BBParser
    //数值更新回调依赖BBParser组件，当不存在组件，不会执行数值回调(IsForced)
    [ComponentOf(typeof(Unit))]
    public class BBNumeric : Entity, IAwake, IDestroy, ISerializeToEntity, ITransfer
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        //note: 万分制!!!
        public Dictionary<string, long> NumericDict = new();
        
        [BsonIgnore]
        public Dictionary<string, long> NumericCallbackDict = new();
    }

    
    
    public struct BBNumericChangedCallback
    {
        public long instanceId;
        public string numericType;
        public long oldValue;
        public long newValue;
    }
}