using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class ScriptParser: Entity, IAwake, IDestroy
    {
        public Dictionary<string, int> funcMap = new();
        public Dictionary<string, int> markerMap = new();
        
        public string opLines;
        public Dictionary<int, string> opDict = new();

        public ETCancellationToken Token;
        //1. parse script to opDict
    }
}