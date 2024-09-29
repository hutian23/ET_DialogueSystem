using System.Numerics;

namespace ET.Client
{
    [ComponentOf(typeof (b2Body))]
    public class RootMotionComponent: Entity, IAwake, IDestroy
    {
        public int currentFrame;

        //dx / 1 = dv
        //运动曲线中当前帧的位移量，等于速度
        public Vector2 velocity;
    }
}