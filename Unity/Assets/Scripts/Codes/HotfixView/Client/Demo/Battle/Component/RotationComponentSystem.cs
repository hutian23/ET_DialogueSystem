using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(RotationComponent))]
    public static class RotationComponentSystem
    {
        public class RotationComponentPostStepSystem : PostStepSystem<RotationComponent>
        {
            protected override void PosStepUpdate(RotationComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
                
                go.transform.eulerAngles = self.EulerAngles;
            }
        }

        public static void SetEulerAngles(this RotationComponent self, Vector3 eulerAngles)
        {
            self.EulerAngles = eulerAngles;
        }
    }
}