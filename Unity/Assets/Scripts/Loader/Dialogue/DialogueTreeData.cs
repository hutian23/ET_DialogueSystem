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
            var cloneNode = MongoHelper.Clone(node);
            cloneNode.text = node.text;
            return cloneNode;
        }
        
        public T GetVariable<T>(string variableName)
        {
            if (!this.variables.TryGetValue(variableName, out var value) || value == null) return default;

            try
            {
                T convertValue = (T)value;
                T cloneValue = MongoHelper.Clone(convertValue);
                return cloneValue;
            }
            catch (Exception)
            {
                Log.Error($"变量{variableName}转换失败!不能将{value.GetType()}转换成{typeof (T)}");
                return default;
            }
        }
    }
}