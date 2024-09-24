namespace ET.Client
{
    [FriendOf(typeof (BehaviorBufferComponent))]
    [FriendOf(typeof (InputCheck))]
    public static class InputCheckSystem
    {
        public static async ETTask InputCheckCor(this InputCheck self, ETCancellationToken token)
        {
            while (true)
            {
                if (!self.GetParent<BehaviorBufferComponent>().behaviorDict.TryGetValue(self.targetID, out BehaviorInfo info))
                {
                    Log.Error($"not found behaviorInfo: {self.targetID}");
                    return;
                }

                Unit unit = self.GetParent<BehaviorBufferComponent>().GetParent<DialogueComponent>().GetParent<Unit>();
                await DialogueDispatcherComponent.Instance.GetBBCheckHandler(self.inputChecker).Handle(unit, token);
                if (token.IsCancel()) return;

                BehaviorBufferComponent bufferComponent = self.GetParent<BehaviorBufferComponent>();
                bufferComponent.AddBuffer(info.GetOrder(), self.LastedFrame, self.targetID);

                await TimerComponent.Instance.WaitFrameAsync(token);
                if (token.IsCancel()) return;
            }
        }
    }
}