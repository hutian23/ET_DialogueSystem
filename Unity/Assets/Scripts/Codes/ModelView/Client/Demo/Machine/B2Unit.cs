using System.Collections.Generic;
using ET.Event;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    // 行为机的物理模块
    // Unit在物理层的映射，负责逻辑层和物理层交互
    [ComponentOf(typeof(Unit))]
    public class B2Unit: Entity, IAwake<long>, IDestroy, IPostStep, IPreStep
    {
        public bool ApplyRootMotion;
        public Queue<CollisionInfo> TriggerBuffer = new(); // 当前帧收集碰撞信息
        public Queue<CollisionInfo> CollisionBuffer = new();
        public Vector2 Velocity; // 和刚体的实际速度区分
        public int Hertz = 60; // 根据TimeScale对速度进行缩放 Hertz / 60
        public long unitId; // 缓存Unit.instanceId(Destroy事件中因为b2Unit未Unit子组件，b2Unit.Dispose时unit.InstanceId = 0)
    }
    
    public struct CreateB2bodyCallback
    {
        public long instanceId;
    }
}