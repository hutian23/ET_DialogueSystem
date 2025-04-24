using UnityEngine;
using Color = Box2DSharp.Common.Color;

namespace ET.Client
{
    [FriendOf(typeof(FollowComponent))]
    [FriendOf(typeof(VirtualCameraManager))]
    public static class FollowComponentSystem
    {
        public class FollowComponentAwakeSystem : AwakeSystem<FollowComponent, long>
        {
            protected override void Awake(FollowComponent self, long instanceId)
            {
                self._instanceId = instanceId;
                self.token = new ETCancellationToken();
                self.FollowCor().Coroutine();
                self.GizmosCor().Coroutine();
            }
        }

        public class FollowComponentDestroySystem : DestroySystem<FollowComponent>
        {
            protected override void Destroy(FollowComponent self)
            {
                self._instanceId = 0;
                self.token.Cancel();
            }
        }

        private static async ETTask FollowCor(this FollowComponent self)
        {
            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            Unit unit = Root.Instance.Get(self._instanceId) as Unit;
            GameObject follow = unit.GetComponent<GameObjectComponent>().GameObject;

            while (true)
            {
                await lateUpdateTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;

                VirtualCameraManager.Instance.Target.transform.position = follow.transform.position;
            }
        }

        private static async ETTask GizmosCor(this FollowComponent self)
        {
            BBTimerComponent gizmosTimer = b2WorldManager.Instance.GetGizmosTimer();

            while (true)
            {
                await gizmosTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;
                
                b2WorldManager.Instance.DrawPoint(VirtualCameraManager.Instance.Target.transform.position.ToVector2(), 9f, Color.Blue);
            }
        }
    }
}