namespace ET.Client
{
    [FriendOf(typeof (ScriptParser))]
    public class RegistMove_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "RegistMove";
        }

        //RegistMove:
        //EndMove:
        public override async ETTask<Status> Handle(Unit unit, ScriptData data, ETCancellationToken token)
        {
            ScriptParser parser = unit.GetComponent<TimelineComponent>().GetComponent<ScriptParser>();

            parser.subCoroutineDatas.TryGetValue(data.coroutineID, out SubCoroutineData coroutineData);
            int pointer = coroutineData.pointer;

            while (++pointer < parser.opDict.Count)
            {
                string opLine = parser.opDict[pointer];
                //EndMove:
                if (opLine.Equals("EndMove:"))
                {
                    break;
                }
            }

            coroutineData.pointer = pointer;
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}