namespace ET.Client
{
    [FriendOf(typeof(IfComponent))]
    [FriendOf(typeof(BBParser))]
    public class Function_BeginIf_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BeginIf";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndIf:"))
                {
                    endIndex = index;
                    break;
                }
            }
            
            // parser.RemoveComponent<IfComponent>();
            // IfComponent ifComponent = parser.AddComponent<IfComponent>();
            //
            // ifComponent.Root = index
                    
                    
                    
                    
                    

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}