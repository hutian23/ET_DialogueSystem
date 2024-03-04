using Sirenix.Utilities;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (BBAnimComponent))]
    public static class BBAnimationComponentSystem
    {
        [Invoke]
        [FriendOf(typeof (BBAnimComponent))]
        public class BBKeyFrameTestCallback: AInvokeHandler<KeyFrameTest>
        {
            public override void Handle(KeyFrameTest args)
            {
                BBAnimComponent bbAnim = Root.Instance.Get(args.instanceId) as BBAnimComponent;
                bbAnim.hitBoxes.ForEach(GameObjectPoolHelper.ReturnObjectToPool); //对象池回收

                bbAnim.SetSprite(args.Keyframe.sprite);
                args.Keyframe.hitBoxInfos.ForEach(hb => { bbAnim.SpawnHitBox(hb); });
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
                self.hitBoxes.ForEach(GameObjectPoolHelper.ReturnObjectToPool); //对象池回收
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
            GameObject go = self
                    .GetParent<DialogueComponent>()
                    .GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject;

            if (hitBoxInfo.type == HitBoxType.None) return;
            GameObject hitbox = GameObjectPoolHelper.GetObjectFromPool("Hitbox");
            self.hitBoxes.Add(hitbox); //组件销毁时，回收hitbox

            string hitType = "";

            switch (hitBoxInfo.type)
            {
                case HitBoxType.HitBox:
                    hitType = "HitBox";
                    hitbox.SetHitBoxColor(Color.red);
                    break;
                case HitBoxType.HurtBox:
                    hitType = "HurtBox";
                    hitbox.SetHitBoxColor(Color.green);
                    break;
                case HitBoxType.PushBox:
                    hitType = "PushBox";
                    hitbox.SetHitBoxColor(Color.yellow);
                    break;
                case HitBoxType.ThrowBox:
                    hitType = "ThrowBox";
                    hitbox.SetHitBoxColor(Color.blue);
                    break;
                case HitBoxType.ThrowHurtBox:
                    hitType = "ThrowHurtBox";
                    hitbox.SetHitBoxColor(Color.magenta);
                    break;
                case HitBoxType.ProximityBox:
                    hitType = "ProximityBox";
                    hitbox.SetHitBoxColor(Color.cyan);
                    break;
            }

            hitbox.transform.SetParent(go.Get<GameObject>($"{hitType}").transform);
            Vector2 localPos = hitBoxInfo.rect.position + new Vector2(hitBoxInfo.rect.size.x, -hitBoxInfo.rect.size.y) / 20;
            hitbox.transform.localPosition = localPos;
            //1 unit等于 10 * 10像素
            hitbox.transform.localScale = hitBoxInfo.rect.size / 10;
        }

        private static void SetHitBoxColor(this GameObject go, Color color)
        {
            Color tmp = new(color.r, color.g, color.b, 0.3f);
            go.GetComponent<SpriteRenderer>().color = tmp;
        }
    }
}