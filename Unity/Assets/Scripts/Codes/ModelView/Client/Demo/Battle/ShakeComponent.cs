using System.Numerics;

namespace ET.Client
{
    [ComponentOf]
    public class ShakeComponent : Entity, IAwake, IDestroy, IFrameLateUpdate
    {
        public Vector2 shakeLength;
        public float frequency;
        public int totalFrame;
        public int curFrame;
        public long unitId;
        public ShakeMode shakeMode;
    }
}