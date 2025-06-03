using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof (BBParser))]
    public class CircleWaveComponent: Entity, IAwake<float, float, int>, IDestroy, IFrameUpdate
    {
        public float waveWidth;
        public float waveSpeed;
        public float progress;
        public int totalTick;
        public int currentTick;
        public MaterialPropertyBlock PropertyBlock;
        public ETCancellationToken token;
    }
}