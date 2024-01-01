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
                self.dispatchHandler.Clear();
                DialogueDispatcherComponent.Instance = null;
            }
        }

        private static void Init(this DialogueDispatcherComponent self)
        {
            self.dispatchHandler.Clear();
            var nodeHandlers = EventSystem.Instance.GetTypes(typeof (DialogueAttribute));
            foreach (Type type in nodeHandlers)
            {
                NodeHandler nodeHandler = Activator.CreateInstance(type) as NodeHandler;
                if (nodeHandler == null)
                {
                    Log.Error($"this nodeHandler is not nodeHandler!: {type.Name}");
                    continue;
                }

                self.dispatchHandler.Add(nodeHandler.GetDialogueType(), nodeHandler);
            }
        }

        public static async ETTask<Status> Handle(this DialogueDispatcherComponent self, Unit unit, object node, ETCancellationToken token)
        {
            if (!self.dispatchHandler.TryGetValue(node.GetType(), out NodeHandler handler))
            {
                throw new Exception($"not found handler: {node}");
            }

            return await handler.Handle(unit, node, token);
        }
    }
}