using System.Numerics;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class TargetCheckComponent : Entity, IAwake, IDestroy, IGizmosUpdate, IPostStep
    {
        public Vector2 center;
        public Vector2 size;
        public bool targetFounded;
        public int findTargetCallback_Index;
        public int loseTargetCallback_Index;
    }
}