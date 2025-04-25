using System.Numerics;
using Box2DSharp.Dynamics;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class BounceCheckComponent : Entity, IAwake<int, Vector2, Vector2>, IDestroy
    {
        public int waitFrame;
        public int curFrame;
        
        public float offsetX;
        public float offsetY;

        public float sizeX;
        public float sizeY;

        public Fixture checkBox;
        
        public ETCancellationToken token;
    }
}