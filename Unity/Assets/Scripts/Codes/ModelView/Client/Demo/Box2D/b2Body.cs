using Box2DSharp.Dynamics;
using UnityEngine;
using Transform = Box2DSharp.Common.Transform;

namespace ET.Client
{
    [ChildOf(typeof (b2GameManager))]
    public class b2Body: Entity, IAwake, IDestroy, ILoad
    {
        public long unitId;

        public bool IsPlayer;

        public Body body;

        //当前步长，b2World中刚体的位置转换信息
        public Transform trans;
    }
}