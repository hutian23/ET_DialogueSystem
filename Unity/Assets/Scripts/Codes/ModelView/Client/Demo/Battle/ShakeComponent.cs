namespace ET.Client
{
    [ComponentOf]
    public class ShakeComponent : Entity, IAwake, IDestroy, IFrameLateUpdate
    {
        public float shakeLength_X;
        public float shakeLength_Y;
        public float frequency;
        public int totalFrame;
        public int curFrame;
        public long unitId;
        
        public ETCancellationToken token;
    }
}