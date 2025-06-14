namespace ET.Client
{
    public static class VirtualCameraSystem
    {
        public class VirtualCameraDestroySystem : DestroySystem<VirtualCamera>
        {
            protected override void Destroy(VirtualCamera self)
            {
                UnityEngine.Object.Destroy(self.gameObject);
            }
        }
    }
}