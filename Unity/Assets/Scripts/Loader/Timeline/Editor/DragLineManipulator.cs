using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public enum DraglineDirection{ Top ,Down,Left,Right}
    
    public class DragLineManipulator : PointerManipulator
    {
        private DraglineDirection m_Direction;
        private Action<PointerDownEvent> m_OnDragStart;
        private Action<Vector2> m_OnDragMove;
        
        public bool Active { get; private set; }
        public IMGUIContainer Handle { get; private set; }

        private Vector3 m_Start;

        public float Size = 4;
        public float Offset = 0;
        public bool Enable = true;

        public DragLineManipulator(DraglineDirection draglineDirection, Action<Vector2> OnDragMove)
        {
            m_OnDragMove = OnDragMove;
            Active = false;
            m_Direction = draglineDirection;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            throw new System.NotImplementedException();
        }
    }
}