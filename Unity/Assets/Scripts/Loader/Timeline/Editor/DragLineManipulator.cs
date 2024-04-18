using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public enum DraglineDirection{ Top ,Down,Left,Right}
    
    public class DragLineManipulator : PointerManipulator
    {
        private readonly DraglineDirection m_Direction;
        private Action<PointerDownEvent> m_OnDragStart;
        private readonly Action m_OnDragStop;
        private readonly Action<Vector2> m_OnDragMove;

        private bool Active { get; set; }
        private IMGUIContainer Handle { get; set; }

        private Vector3 m_Start;

        public float Size = 4;
        private readonly float Offset = 0;
        private readonly bool Enable = true;

        public DragLineManipulator(DraglineDirection draglineDirection, Action<Vector2> OnDragMove)
        {
            m_OnDragMove = OnDragMove;
            Active = false;
            m_Direction = draglineDirection;
            activators.Add(new ManipulatorActivationFilter()
            {
                button = MouseButton.LeftMouse
            });
        }

        public DragLineManipulator(DraglineDirection dragLineDirection, Action<Vector2> onDragMove, Action<PointerDownEvent> OnDragStart, Action OnDragStop): this(dragLineDirection, onDragMove)
        {
            m_OnDragStart = OnDragStart;
            m_OnDragStop = OnDragStop;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            Handle = new IMGUIContainer(() =>
            {
                if (!Enable || Active)
                {
                    return;
                }

                switch (m_Direction)
                {
                    case DraglineDirection.Top:
                        EditorGUIUtility.AddCursorRect(new Rect(0,0,target.worldBound.width,Size),MouseCursor.ResizeVertical);
                        break;
                    case DraglineDirection.Down:
                        EditorGUIUtility.AddCursorRect(new Rect(0,0,target.worldBound.width,Size),MouseCursor.ResizeVertical);
                        break;
                    case DraglineDirection.Left:
                        EditorGUIUtility.AddCursorRect(new Rect(0,0,target.worldBound.height,Size), MouseCursor.ResizeHorizontal);
                        break;
                    case DraglineDirection.Right:
                        EditorGUIUtility.AddCursorRect(new Rect(0,0,Size,target.worldBound.height),MouseCursor.ResizeHorizontal);
                        break;
                }
            });
            Handle.style.position = Position.Absolute;
            Handle.style.marginTop = Handle.style.marginBottom = Handle.style.marginLeft = Handle.style.marginRight = 0;
            switch (m_Direction)
            {
                case DraglineDirection.Top:
                    Handle.style.top = (-target.style.borderTopWidth.value + Offset);
                    Handle.style.width = Length.Percent(100);
                    Handle.style.height = Size;
                    break;
                case DraglineDirection.Down:
                    Handle.style.bottom = (-target.style.borderBottomWidth.value + Offset);
                    Handle.style.width = Length.Percent(100);
                    Handle.style.height = Size;
                    break;
                case DraglineDirection.Left:
                    Handle.style.left = (-target.style.borderLeftWidth.value + Offset);
                    Handle.style.width = Size;
                    Handle.style.height = Length.Percent(100);
                    break;
                case DraglineDirection.Right:
                    Handle.style.right = (-target.style.borderRightWidth.value + Offset);
                    Handle.style.width = Size;
                    Handle.style.height = Length.Percent(100);
                    break;
            }
            Handle.RegisterCallback<PointerDownEvent>(OnPointerDown);
            Handle.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            Handle.RegisterCallback<PointerUpEvent>(OnPointerUp);
            target.Add(Handle);
        }
        
        protected override void UnregisterCallbacksFromTarget()
        {   
            Handle.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            Handle.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            Handle.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            
            target.Remove(Handle);
            Handle = null;
        }

        private void OnPointerDown(PointerDownEvent e)
        {
            if (!Enable) return;
            if (Active)
            {
                //阻止其他Pointer事件,比如moveClip
                e.StopImmediatePropagation();
            }
            else if (CanStartManipulation(e))
            {
                m_Start = e.localPosition;
                Active = true;
                Handle.CapturePointer(e.pointerId);
                e.StopPropagation();
                
                m_OnDragStart?.Invoke(e);
            }
        }

        private void OnPointerMove(PointerMoveEvent e)
        {
            if (!Enable) return;

            if (Active && Handle.HasPointerCapture(e.pointerId))
            {
                Vector2 delta = e.localPosition - m_Start;
                ApplyDelta(delta);
                e.StopPropagation();
            }
        }
        
        private void OnPointerUp(PointerUpEvent e)
        {
            if (!Enable) return;

            if (Active && CanStopManipulation(e))
            {
                Active = false;
                Handle.ReleasePointer(e.pointerId);
                e.StopPropagation();
                
                m_OnDragStop?.Invoke();
            }
        }
        
        private void ApplyDelta(Vector2 delta)
        {
            m_OnDragMove?.Invoke(delta);
        }
    }
}