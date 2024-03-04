using UnityEngine;

namespace ET.Client
{
    public static class BBAnimationComponentSystem
    {
        [Invoke]
        public class BBKeyFrameTestCallback: AInvokeHandler<KeyFrameTest>
        {
            public override void Handle(KeyFrameTest args)
            {
                BBAnimComponent bbAnim = Root.Instance.Get(args.instanceId) as BBAnimComponent;
                bbAnim.SetSprite(args.Keyframe.sprite);
                
            }
        }

        [Invoke(BBTimerInvokeType.AnimTimer)]
        public class BBAnimTimer: BBTimer<BBAnimComponent>
        {
            protected override void Run(BBAnimComponent self)
            {
                
            }
        }

        public class BBAnimationComponentAwakeSystem: AwakeSystem<BBAnimComponent>
        {
            protected override void Awake(BBAnimComponent self)
            {
                self.timer = self.GetParent<DialogueComponent>().GetComponent<BBTimerComponent>().NewFrameTimer(BBTimerInvokeType.AnimTimer, self);
                GameObject go = self.GetParent<DialogueComponent>().GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
                BBAnimViewComponent animComponent = go.AddComponent<BBAnimViewComponent>();
                animComponent.instanceId = self.InstanceId;

                GameObjectPoolHelper.InitPool("Hitbox", 10);
            }
        }

        public class BBAnimationComponentDestroySystem: DestroySystem<BBAnimComponent>
        {
            protected override void Destroy(BBAnimComponent self)
            {
                GameObject go = self.GetParent<DialogueComponent>().GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
                UnityEngine.Object.Destroy(go.GetComponent<BBAnimViewComponent>());
            }
        }

        private static void SetSprite(this BBAnimComponent self, Sprite sprite)
        {
            self.GetParent<DialogueComponent>()
                    .GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<SpriteRenderer>()
                    .sprite = sprite;
        }

        private static void SpawnHitBox(this BBAnimComponent self, HitBoxInfo hitBoxInfo)
        {
            
        }
    }
}