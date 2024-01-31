using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Client
{
    public class DialogueTreeData
    {
        private readonly Dictionary<uint, DialogueNode> targets = new();
        private readonly Dictionary<string, object> variables = new();

        public DialogueTreeData(BsonDocument document, Language language)
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
                    targets.Add((uint)i, node);
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
            if (!targets.TryGetValue(targetID, out DialogueNode node)) return null;
            return node;
        }

        public T GetVariable<T>(string variableName)
        {
            if (!this.variables.TryGetValue(variableName, out var value)) return default;
            object cloneValue = MongoHelper.Clone(value);
            return (T)cloneValue;
        }
    }
}