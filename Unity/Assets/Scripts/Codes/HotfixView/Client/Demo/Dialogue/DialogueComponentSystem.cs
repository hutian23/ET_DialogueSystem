namespace ET.Client
{
    [FriendOf(typeof (DialogueComponent))]
    public static class DialogueComponentSystem
    {
        public class DialogueComponentLoadSystem: LoadSystem<DialogueComponent>
        {
            protected override void Load(DialogueComponent self)
            {
                self.Init();
                self.token = new ETCancellationToken();
                if (self.tree == null) return;
                self.DialogueCor().Coroutine();
            }
        }

        public class DialogueComponentDestroySystem : DestroySystem<DialogueComponent>
        {
            protected override void Destroy(DialogueComponent self)
            {
                self.Init();
            }
        }

        public static void LoadTree(this DialogueComponent self,DialogueTree tree)
        {
            self.token?.Cancel();
            self.token = new ETCancellationToken();
            self.workQueue.Clear();
            self.tree = tree;
            self.DialogueCor().Coroutine();
        }
        
        private static void Init(this DialogueComponent self)
        {
            self.token?.Cancel();
            self.token = null;
            self.workQueue.Clear();
        }

        public static async ETTask<Status> DialogueCor(this DialogueComponent self)
        {
            self.workQueue.Enqueue(self.tree.root);

            Status status = Status.Success;
            Unit unit = self.GetParent<Unit>();

            while (self.workQueue.Count != 0)
            {
                if (self.token.IsCancel()) break;
                //将下一个节点压入queue并执行
                DialogueNode node = self.workQueue.Dequeue();
                Status ret = await DialogueDispatcherComponent.Instance.Handle(unit, node, self.token);
                if (self.token.IsCancel() || ret == Status.Failed)
                {
                    status = Status.Failed;
                    break;
                }

                await TimerComponent.Instance.WaitFrameAsync(self.token);
            }

            self.Init();
            return status;
        }
    }
}