namespace ET.Client
{
    public class TimelineCor_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "StartTimelineCor";
        }

        //StartTimelineCor;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = parser.GetParent<DialogueComponent>();
            BBTimerComponent bbTimer = dialogueComponent.GetComponent<BBTimerComponent>();
            RootMotionComponent rootMotion = dialogueComponent.GetComponent<RootMotionComponent>();
            PlayableManager playableManager = dialogueComponent.GetComponent<PlayableManager>();

            rootMotion.Init(data.targetID);

            for (int i = 0; i <= playableManager.GetPlayable().ClipMaxFrame(); i++)
            {
                playableManager.GetPlayable().Evaluate(i);
                
                //Update root motion
                rootMotion.UpdatePos(i);
                
                await bbTimer.WaitAsync(1, token);
                if (token.IsCancel())
                {
                    return Status.Failed;
                }
            }

            rootMotion.OnDone();
            
            return Status.Success;
        }
    }
}