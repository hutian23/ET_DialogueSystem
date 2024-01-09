using System;

namespace ET.Client
{
    [FriendOf(typeof (DialogueDispatcherComponent))]
    public static class DialogueDispatcherComponentSystem
    {
        public class DialogueDispatcherComponentAwakeSystem: AwakeSystem<DialogueDispatcherComponent>
        {
            protected override void Awake(DialogueDispatcherComponent self)
            {
                DialogueDispatcherComponent.Instance = self;
                self.Init();
            }
        }

        public class DialogueDispatcherComponentLoadSystem: LoadSystem<DialogueDispatcherComponent>
        {
            protected override void Load(DialogueDispatcherComponent self)
            {
                self.Init();
            }
        }

        public class DialogueDispatcherComponentDestorySystem: DestroySystem<DialogueDispatcherComponent>
        {
            protected override void Destroy(DialogueDispatcherComponent self)
            {
                self.dispatchHandlers.Clear();
                DialogueDispatcherComponent.Instance = null;
            }
        }

        private static void Init(this DialogueDispatcherComponent self)
        {
            self.dispatchHandlers.Clear();
            var nodeHandlers = EventSystem.Instance.GetTypes(typeof (DialogueAttribute));
            foreach (Type type in nodeHandlers)
            {
                NodeHandler nodeHandler = Activator.CreateInstance(type) as NodeHandler;
                if (nodeHandler == null)
                {
                    Log.Error($"this nodeHandler is not nodeHandler!: {type.Name}");
                    continue;
                }

                self.dispatchHandlers.Add(nodeHandler.GetDialogueType(), nodeHandler);
            }

            self.checker_dispatchHandlers.Clear();
            var nodeCheckerHandlers = EventSystem.Instance.GetTypes(typeof (NodeCheckerAttribute));
            foreach (Type type in nodeCheckerHandlers)
            {
                NodeCheckHandler nodeCheckHandler = Activator.CreateInstance(type) as NodeCheckHandler;
                if (nodeCheckHandler == null)
                {
                    Log.Error($"this obj is not a nodeCheckerHandler:{type.Name}");
                    continue;
                }
                self.checker_dispatchHandlers.Add(nodeCheckHandler.GetNodeCheckType(), nodeCheckHandler);
            }
        }

        public static async ETTask<Status> Handle(this DialogueDispatcherComponent self, Unit unit, object node, ETCancellationToken token)
        {
            if (!self.dispatchHandlers.TryGetValue(node.GetType(), out NodeHandler handler))
            {
                Log.Error($"not found handler: {node}");
                return Status.Failed;
            }

            return await handler.Handle(unit, node, token);
        }

        public static int Check(this DialogueDispatcherComponent self, Unit unit, object nodeCheckConfig)
        {
            if (!self.checker_dispatchHandlers.TryGetValue(nodeCheckConfig.GetType(), out NodeCheckHandler nodeCheckerHandler))
            {
                throw new Exception($"not found nodeCheckerHandler: {nodeCheckConfig}");
            }

            return nodeCheckerHandler.Check(unit, nodeCheckConfig);
        }
    }
}