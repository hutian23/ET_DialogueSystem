using System;
using System.Collections.Generic;
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
            if (Application.isEditor)
            {
                self.cloneTree = self.tree.DeepClone();
                DialogueViewComponent view = self.GetParent<Unit>()
                        .GetComponent<GameObjectComponent>().GameObject
                        .GetComponent<DialogueViewComponent>();
                view.cloneTree = self.cloneTree;
                view.tree = self.tree;
            }
            else
            {
                self.targets = self.tree.CloneTargets();
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
            DialogueNode node = self.cloneTree.root;
            self.workQueue.Enqueue(node);

            Status status = Status.Success;
            Unit unit = self.GetParent<Unit>();

            try
            {
                while (self.workQueue.Count != 0)
                {
                    if (self.token.IsCancel()) break;
                    //将下一个节点压入queue并执行
                    node = self.workQueue.Dequeue();

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
                    //存档
                    DialogueStorageManager.Instance.QuickSaveShot.AddToBuffer(self.tree.treeID, node.TargetID);
                    await TimerComponent.Instance.WaitFrameAsync(self.token);
                }

                self.Init();
            }
            catch (Exception e)
            {
                Log.Error(e);
                node.Status = Status.Failed;
            }

            return status;
        }

        public static DialogueNode GetNode(this DialogueComponent self, uint targetID)
        {
            DialogueNode node;
            if (UnityEngine.Application.isEditor)
                self.cloneTree.targets.TryGetValue(targetID, out node);
            else
                self.targets.targets.TryGetValue(targetID, out node);
            if (node == null) Log.Error($"not found target node! :{targetID}");
            return node;
        }

        public static void PushNextNode(this DialogueComponent self, DialogueNode node)
        {
            if (node == null) return;
            self.workQueue.Enqueue(node);
        }

        public static void PushNextNode(this DialogueComponent self, uint targetID)
        {
            DialogueNode node = self.GetNode(targetID);
            self.PushNextNode(node);
        }

        public static void PushNextNode(this DialogueComponent self, List<uint> targets)
        {
            targets.ForEach(self.PushNextNode);
        }
    }
}