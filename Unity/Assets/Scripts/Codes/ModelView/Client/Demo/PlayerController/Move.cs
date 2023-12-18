using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Buffer))]
    public class Move : Entity,IAwake,IDestroy
    {
        public Vector2Int move;
    }
}