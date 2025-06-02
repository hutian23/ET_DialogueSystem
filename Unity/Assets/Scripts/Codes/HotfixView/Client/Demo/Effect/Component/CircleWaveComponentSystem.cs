namespace ET.Client
{
    public static class CircleWaveComponentSystem
    {
        public class CircleWaveComponentAwakeSystem : AwakeSystem<CircleWaveComponent, float, float, int>
        {
            protected override void Awake(CircleWaveComponent self, float waveSpeed, float waveWidth, int totalTick)
            {
                self.waveSpeed = waveSpeed;
                self.waveWidth = waveWidth;
                self.totalTick = totalTick;
                self.currentTick = 0;
            }
        }
        
        public class CircleWaveComponentFrameUpdateSystem : FrameUpdateSystem<CircleWaveComponent>
        {
            protected override void FrameUpdate(CircleWaveComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                CircleWaveController controller = unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<CircleWaveController>();
                controller.WaveChange(self.waveWidth, self.waveSpeed, self.currentTick ++);
            }
        }
        
        public class CircleWaveComponentDestroySystem : DestroySystem<CircleWaveComponent>
        {
            protected override void Destroy(CircleWaveComponent self)
            {
                self.waveSpeed = 0f;
                self.waveWidth = 0f;
                self.totalTick = 0;
                self.currentTick = 0;
            }
        }
    }
}