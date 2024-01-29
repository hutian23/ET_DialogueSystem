#if UNITY
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using UnityEngine;

namespace ET
{
    public class KeyframeSerializer: MongoHelper.StructBsonSerialize<Keyframe>
    {
        public override Keyframe Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var doc = BsonSerializer.Deserialize<BsonDocument>(context.Reader);
            BsonDocument _v = doc.TryGetValue("_v", out var v)? v.ToBsonDocument() : doc; //直接序列化Keyframe实例 or AnimationCure中的Keyframe实例

            return new Keyframe()
            {
                time = (float)_v["m_Time"].AsDouble,
                value = (float)_v["m_Value"].AsDouble,
                inTangent = (float)_v["m_InTangent"].AsDouble,
                outTangent = (float)_v["m_OutTangent"].AsDouble,
                tangentModeInternal = _v["m_TangentMode"].AsInt32,
                inWeight = (float)_v["m_InWeight"].AsDouble,
                outWeight = (float)_v["m_OutWeight"].AsDouble
            };
        }
    }
}
#endif