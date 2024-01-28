using System;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using UnityEngine;

namespace ET
{
        public class KeyframeBsonSerializer : StructSerializerBase<Keyframe>
        {
            public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Keyframe value)
            {
                var doc = new BsonDocument
                {
                    { nameof(Keyframe.time), value.time },
                    { nameof(Keyframe.value), value.value },
                    { nameof(Keyframe.inTangent), value.inTangent },
                    { nameof(Keyframe.outTangent), value.outTangent },
                    { nameof(Keyframe.inWeight), value.inWeight },
                    { nameof(Keyframe.outWeight), value.outWeight },
                    { nameof(Keyframe.weightedMode), (int)value.weightedMode },
                    { nameof(Keyframe.tangentModeInternal), value.tangentModeInternal }
                };

                BsonSerializer.Serialize(context.Writer, typeof(BsonDocument), doc);
            }

            public override Keyframe Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            {
                var Totoaldoc = BsonSerializer.Deserialize<BsonDocument>(context.Reader);
                Debug.Log(Totoaldoc);
                var doc = Totoaldoc["_v"].ToBsonDocument();
                Debug.Log(doc);
                Debug.Log(doc["time"]);
                return new Keyframe
                {
                    time = doc.GetValue("time").AsInt64
                };
            }
        }
}