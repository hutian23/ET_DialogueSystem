using UnityEngine.Device;

namespace ET.Client
{
    [FriendOf(typeof (DialogueComponent))]
    public static class DialogueComponentSystem
    {
        public class DialogueComponentAwakeSystem: AwakeSystem<DialogueComponent>
        {
            protected override void Awake(DialogueComponent self)
            {
                //这里不知道为什么不能使用UNITY_EDITOR
                if (Application.isEditor)
                {
                    self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject.AddComponent<DialogueViewComponent>();
                }
            }
        }

        public class DialogueComponentLoadSystem: LoadSystem<DialogueComponent>
        {
            protected override void Load(DialogueComponent self)
            {
                self.Init();
                self.token = new ETCancellationToken();

                // self.cloneTree = MongoHelper.Clone(self.tree);
                // if (Application.isEditor)
                // {
                //     DialogueViewComponent view = self.GetParent<Unit>()
                //             .GetComponent<GameObjectComponent>().GameObject
                //             .GetComponent<DialogueViewComponent>();
                //     view.cloneTree = self.cloneTree;
                // }

                if (Application.isEditor)
                {
                    //热重载更新Status
                    self.cloneTree.root.Status = Status.None;
                    self.cloneTree.nodes.ForEach(node => node.Status = Status.None);   
                }
                
                if (self.tree == null) return;
                self.DialogueCor().Coroutine();
            }
        }

        public class DialogueComponentDestroySystem: DestroySystem<DialogueComponent>
        {
            protected override void Destroy(DialogueComponent self)
            {
                self.Init();
                self.cloneTree = null;
                self.tree = null;
            }
        }

        public static void LoadTree(this DialogueComponent self, DialogueTree tree)
        {
            self.token?.Cancel();
            self.token = new ETCancellationToken();
            self.workQueue.Clear();

            self.tree = tree;
            self.cloneTree = self.tree.DeepClone();
            if (Application.isEditor)
            {
                DialogueViewComponent view = self.GetParent<Unit>()
                        .GetComponent<GameObjectComponent>().GameObject
                        .GetComponent<DialogueViewComponent>();
                view.cloneTree = self.cloneTree;
                view.tree = self.tree;
            }

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
            self.workQueue.Enqueue(self.cloneTree.root);

            Status status = Status.Success;
            Unit unit = self.GetParent<Unit>();

            while (self.workQueue.Count != 0)
            {
                if (self.token.IsCancel()) break;
                //将下一个节点压入queue并执行
                DialogueNode node = self.workQueue.Dequeue();

                if (Application.isEditor) node.Status = Status.Pending;

                Status ret = await DialogueDispatcherComponent.Instance.Handle(unit, node, self.token);
                //携程取消 or 执行失败
                if (self.token.IsCancel() || ret == Status.Failed)
                {
                    status = Status.Failed;
                    if (Application.isEditor) node.Status = Status.Failed;
                    break;
                }

                if (Application.isEditor) node.Status = Status.Success;

                await TimerComponent.Instance.WaitFrameAsync(self.token);
            }

            self.Init();
            return status;
        }

        public static DialogueNode GetNode(this DialogueComponent self, int targetID)
        {
            self.cloneTree.targets.TryGetValue(targetID, out DialogueNode node);
            return node;
        }

        public static void PushNextNode(this DialogueComponent self, DialogueNode node)
        {
            if (node == null) return;
            self.workQueue.Enqueue(node);
        }

        public static void PushNextNode(this DialogueComponent self, int targetID)
        {
            DialogueNode node = self.GetNode(targetID);
            self.PushNextNode(node);
        }
    }
}