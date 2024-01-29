using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Client
{
    public class DialogueTreeData
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<uint, DialogueNode> targets = new();
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<string, object> variables = new();

        public DialogueTreeData(BsonDocument document,Language language)
        {
            try
            {
                var targetDoc = document["targets"].ToBsonDocument();
                int length = targetDoc["Length"].AsInt32;
                for (int i = 0; i < length; i++)
                {
                    var nodeDoc = targetDoc[i].ToBsonDocument();
                    DialogueNode node = MongoHelper.Deserialize<DialogueNode>(nodeDoc.ToBson());
                    node.FromID(nodeDoc.GetValue("ID").AsInt64);

                    var contentDoc = nodeDoc.GetValue("content").ToBsonDocument();
                    node.text = contentDoc[(int)language].AsString;
                    targets.Add((uint)i,node);
                }

                var variablesDoc = document["variables"].ToBsonDocument();
                length = variablesDoc["Length"].AsInt32;
                for (int i = 0; i < length; i++)
                {
                    var variableDoc = variablesDoc[i].ToBsonDocument();
                    SharedVariable sharedVariable = MongoHelper.Deserialize<SharedVariable>(variableDoc.ToBson());
                    variables.TryAdd(sharedVariable.name, sharedVariable.value);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public DialogueNode GetNode(uint targetID)
        {
            if (targets.TryGetValue(targetID, out DialogueNode node))
            {
                return node;
            }

            return null;
        }

        public T GetVariable<T>(string variableName)
        {
            if (variables.TryGetValue(variableName, out var value))
            {
                return (T)value;
            }

            return default;
        }
    }
}