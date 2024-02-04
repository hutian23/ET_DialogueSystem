using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (DialogueComponent))]
    public static class DialogueComponentSystem
    {
        [Invoke]
        [FriendOf(typeof (DialogueComponent))]
        public class ReloadCallback: AInvokeHandler<ViewComponentReloadCallback>
        {
            public override void Handle(ViewComponentReloadCallback args)
            {
                if (!Application.isEditor) return;

                DialogueComponent dialogueComponent = Root.Instance.Get(args.instanceId) as DialogueComponent;
                dialogueComponent.Init();
                dialogueComponent.token = new ETCancellationToken();
                dialogueComponent.ReloadType = args.ReloadType;
                DialogueHelper.Reload(); // 重载
                
                switch (args.ReloadType)
                {
                    case ViewReloadType.Preview:
                        dialogueComponent.PreviewCor(dialogueComponent.GetNode(args.preView_TargetID)).Coroutine();
                        break;
                    case ViewReloadType.RuntimeReload:
                        dialogueComponent.LoadTree(args.treeName, args.language);
                        break;
                    default:
                        dialogueComponent.DialogueCor().Coroutine();
                        break;
                }
            }
        }

        private static void ViewStatusReset(this DialogueComponent self)
        {
            if (!Application.isEditor) return;
            DialogueViewComponent viewComponent = self.GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<DialogueViewComponent>();
            viewComponent.cloneTree.root.Status = Status.None;
            viewComponent.cloneTree.nodes.ForEach(node => { node.Status = Status.None; });
        }

        public static void SetNodeStatus(this DialogueComponent self, DialogueNode node, Status status)
        {
            if (!Application.isEditor || self.ReloadType == ViewReloadType.RuntimeReload) return;
            DialogueViewComponent viewComponent = self.GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<DialogueViewComponent>();
            DialogueNode sourceNode = viewComponent.cloneTree.targets.Values.FirstOrDefault(n => n.Guid == node.Guid);
            sourceNode.Status = status;
        }

        public class DialogueComponentAwakeSystem: AwakeSystem<DialogueComponent>
        {
            protected override void Awake(DialogueComponent self)
            {
                if (Application.isEditor)
                {
                    DialogueViewComponent viewComponent = self.GetParent<Unit>()
                            .GetComponent<GameObjectComponent>().GameObject
                            .AddComponent<DialogueViewComponent>();
                    viewComponent.instanceId = self.InstanceId;
                }

                self.AddComponent<ObjectWait>();
            }
        }
        
        public class DialogueComponentDestroySystem: DestroySystem<DialogueComponent>
        {
            protected override void Destroy(DialogueComponent self)
            {
                self.Init();
                self.treeData = null;
            }
        }

        private static void Init(this DialogueComponent self)
        {
            self.token?.Cancel();
            self.token = null;
            self.workQueue.Clear();
            self.tags.Clear();
        }

        //运行时使用
        public static void LoadTree(this DialogueComponent self, string treeName, Language language)
        {
            self.Init();
            self.token = new ETCancellationToken();
            self.treeData = DialogueHelper.LoadDialogueTree(treeName, language);
            self.DialogueCor().Coroutine();
        }

        private static async ETTask PreviewCor(this DialogueComponent self, DialogueNode preViewNode)
        {
            await TimerComponent.Instance.WaitFrameAsync();
            if (Application.isEditor) self.ViewStatusReset();

            DialogueNode node = self.GetNode(0);
            Unit unit = self.GetParent<Unit>();
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node, self.token);
            self.workQueue.Enqueue(preViewNode);
            try
            {
                while (self.workQueue.Count != 0)
                {
                    if (self.token.IsCancel()) break;
                    node = self.workQueue.Dequeue(); //将下一个节点压入queue执行

                    self.SetNodeStatus(node, Status.Pending);
                    Status ret = await DialogueDispatcherComponent.Instance.Handle(unit, node, self.token);
                    self.SetNodeStatus(node, ret);

                    if (self.token.IsCancel() || ret == Status.Failed) break; //携程取消 or 执行失败
                    await TimerComponent.Instance.WaitFrameAsync(self.token);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private static async ETTask DialogueCor(this DialogueComponent self)
        {
            await TimerComponent.Instance.WaitFrameAsync(); // 意义?: 等待所有reload生命周期事件执行完毕
            if (Application.isEditor) self.ViewStatusReset();

            DialogueNode node = self.GetNode(0); //压入根节点
            self.workQueue.Enqueue(node);
            Unit unit = self.GetParent<Unit>();

            try
            {
                while (self.workQueue.Count != 0)
                {
                    if (self.token.IsCancel()) break;
                    node = self.workQueue.Dequeue(); //将下一个节点压入queue执行

                    self.SetNodeStatus(node, Status.Pending);
                    Status ret = await DialogueDispatcherComponent.Instance.Handle(unit, node, self.token);
                    self.SetNodeStatus(node, ret);

                    if (self.token.IsCancel() || ret == Status.Failed) break; //携程取消 or 执行失败
                    await TimerComponent.Instance.WaitFrameAsync(self.token);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static DialogueNode GetNode(this DialogueComponent self, uint targetID)
        {
            if (Application.isEditor)
            {
                switch (self.ReloadType)
                {
                    case ViewReloadType.RuntimeReload:
                        return self.treeData.GetNode(targetID);
                    default:
                        return self.GetParent<Unit>()
                                .GetComponent<GameObjectComponent>().GameObject
                                .GetComponent<DialogueViewComponent>().GetNode(targetID);
                }
            }

            return self.treeData.GetNode(targetID);
        }

        public static void PushNextNode(this DialogueComponent self, DialogueNode node)
        {
            if (node == null || node.TargetID == 0) return; //注意不能压入根节点
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

        public static T GetVariable<T>(this DialogueComponent self, string variableName)
        {
            if (Application.isEditor)
            {
                switch (self.ReloadType)
                {
                    case ViewReloadType.RuntimeReload:
                        return self.treeData.GetVariable<T>(variableName);
                    default:
                        return self.GetParent<Unit>()
                                .GetComponent<GameObjectComponent>().GameObject
                                .GetComponent<DialogueViewComponent>().cloneTree.GetVariable<T>(variableName);
                }
            }

            return self.treeData.GetVariable<T>(variableName);
        }

        public static void AddTag(this DialogueComponent self, int tagType)
        {
            self.tags.Add(tagType);
        }

        public static bool ContainTag(this DialogueComponent self, int tagType)
        {
            return self.tags.Contains(tagType);
        }

        public static void RemoveTag(this DialogueComponent self, int tagType)
        {
            self.tags.Remove(tagType);
        }
    }
}