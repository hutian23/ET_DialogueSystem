using System.Collections.Generic;
using ET.Event;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class SceneBoxHandler : Entity, IAwake, IDestroy, IPostStep, IGizmosUpdate
    {
        public Queue<CollisionInfo> TriggerEnterQueue = new();
        public Queue<CollisionInfo> TriggerStayQueue = new();
        public Queue<CollisionInfo> TriggerExitQueue = new();
        public Queue<CollisionInfo> CollisionEnterQueue = new();
        public Queue<CollisionInfo> CollisionStayQueue = new();
        public Queue<CollisionInfo> CollisionExitQueue = new();

        public CollisionInfo info;
    }
}