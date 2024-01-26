using System;
using System.Collections.Generic;
using Application = UnityEngine.Device.Application;

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

                switch (args.ReloadType)
                {
                    case ViewReloadType.Preview:
                        //刷新status
                        dialogueComponent.ViewStatusReset();
                        dialogueComponent.PreviewCor(dialogueComponent.GetNode(args.preView_TargetID)).Coroutine();
                        break;
                    default:
                        dialogueComponent.DialogueCor().Coroutine();
                        break;
                }
            }
        }

        private static void ViewStatusReset(this DialogueComponent self)
        {
            DialogueViewComponent viewComponent = self.GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<DialogueViewComponent>();
            viewComponent.cloneTree.root.Status = Status.None;
            viewComponent.cloneTree.nodes.ForEach(node => { node.Status = Status.None; });
        }

        public class DialogueComponentAwakeSystem: AwakeSystem<DialogueComponent>
        {
            protected override void Awake(DialogueComponent self)
            {
                if (Application.isEditor)
                {
                    DialogueViewComponent viewComponent = self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject
                            .AddComponent<DialogueViewComponent>();
                    viewComponent.instanceId = self.InstanceId;
                }
            }
        }

        public class DialogueComponentLoadSystem: LoadSystem<DialogueComponent>
        {
            protected override void Load(DialogueComponent self)
            {
                self.Init();
                self.token = new ETCancellationToken();

                if (Application.isEditor) self.ViewStatusReset();
                self.DialogueCor().Coroutine();
            }
        }

        public class DialogueComponentDestroySystem: DestroySystem<DialogueComponent>
        {
            protected override void Destroy(DialogueComponent self)
            {
                self.Init();
            }
        }

        private static void Init(this DialogueComponent self)
        {
            self.token?.Cancel();
            self.token = null;
            self.workQueue.Clear();
        }

        //运行时使用
        public static void LoadTree(this DialogueComponent self, string treeName, Language language)
        {
            self.Init();
            self.token = new ETCancellationToken();

            self.targets = DialogueHelper.LoadDialogueTree(treeName, language);
            self.DialogueCor().Coroutine();
        }

        private static async ETTask PreviewCor(this DialogueComponent self, DialogueNode preViewNode)
        {
            await TimerComponent.Instance.WaitFrameAsync();

            DialogueNode node = self.GetNode(0);
            Unit unit = self.GetParent<Unit>();
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node.Script, self.token);
            self.workQueue.Enqueue(preViewNode);
            try
            {
                while (self.workQueue.Count != 0)
                {
                    if (self.token.IsCancel()) break;
                    node = self.workQueue.Dequeue(); //将下一个节点压入queue执行

                    if (Application.isEditor) node.Status = Status.Pending;
                    Status ret = await DialogueDispatcherComponent.Instance.Handle(unit, node, self.token);
                    node.Status = ret;

                    if (self.token.IsCancel() || ret == Status.Failed) break; //携程取消 or 执行失败
                    await TimerComponent.Instance.WaitFrameAsync(self.token);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static async ETTask DialogueCor(this DialogueComponent self)
        {
            await TimerComponent.Instance.WaitFrameAsync(); // 意义?: 等待所有reload生命周期事件执行完毕

            DialogueNode node = self.GetNode(0); //压入根节点
            self.workQueue.Enqueue(node);
            Unit unit = self.GetParent<Unit>();

            try
            {
                while (self.workQueue.Count != 0)
                {
                    if (self.token.IsCancel()) break;
                    node = self.workQueue.Dequeue(); //将下一个节点压入queue执行

                    if (Application.isEditor) node.Status = Status.Pending;
                    Status ret = await DialogueDispatcherComponent.Instance.Handle(unit, node, self.token);
                    node.Status = ret;

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
                return self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject.GetComponent<DialogueViewComponent>().GetNode(targetID);
            return self.targets[targetID];
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