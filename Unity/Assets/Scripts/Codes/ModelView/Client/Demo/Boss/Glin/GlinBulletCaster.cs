using System.Numerics;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class GlinBulletCaster : Entity, IAwake, IDestroy
    {
        public int interval;
        public Vector2 targetPos;
        public int cnt;
        public float offset;

        public float shakeLengthX;
        public float shakeLengthY;
        public float frequency;
        public int shakeFrame;
        
        public ETCancellationToken token;
    }
}