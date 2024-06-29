using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public sealed class DragManipulator: IManipulator
    {
        private VisualElement _target;

        public VisualElement target
        {
            get => _target;
            set
            {
                if (_target != null)
                {
                    if (_target == value)
                    {
                        return;
                    }

                    _target.UnregisterCallback<PointerDownEvent>(DragBegin);
                    _target.UnregisterCallback<PointerUpEvent>(DragEnd);
                    _target.UnregisterCallback<PointerMoveEvent>(PointerMove);
                    _target.UnregisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
                    _target.RemoveFromClassList("draggable");
                    lastDroppable?.RemoveFromClassList("droppable--can-drop");
                    lastDroppable = null;
                }

                _target = value;

                _target.RegisterCallback<PointerDownEvent>(DragBegin);
                _target.RegisterCallback<PointerUpEvent>(DragEnd);
                _target.RegisterCallback<PointerMoveEvent>(PointerMove);
                _target.RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
                _target.AddToClassList("draggable");
            }
        }

        private static readonly CustomStyleProperty<bool> draggableEnableProperty = new("--draggable-enabled");
        private Vector3 offset;
        private bool isDragging;
        private VisualElement lastDroppable;
        private string _droppabled = "droppable";

        /** This is the USS class that is determines whether the target can be dropped on it. It is "droppable" by default. */
        public string droppableId
        {
            get => _droppabled;
            set => _droppabled = value;
        }

        /** This manipulator can be disabled. */
        public bool enabled { get; set; } = true;
        private PickingMode lastPickingMode;
        private string _removeClassOnDrag;
        /** Optional. Remove the given class from the target element during the drag. If removed, replace when drag ends. */
        public string removeClassOnDrag
        {
            get => _removeClassOnDrag;
            set => _removeClassOnDrag = value;
        }

        private bool removeClass;
        private readonly bool m_CheckDroppable;
        private readonly int m_Button;
        
        private readonly Action<PointerDownEvent> OnDrag;
        private readonly Action OnDrop;
        private readonly Action<Vector2> OnMove;

        private DragManipulator(bool checkDroppable, int button = 0)
        {
            m_Button = button;
            m_CheckDroppable = checkDroppable;
        }

        public DragManipulator(Action<PointerDownEvent> onDrag, Action onDrop, Action<Vector2> onMove, int button = 0): this(false, button)
        {
            OnDrag = onDrag;
            OnDrop = onDrop;
            OnMove = onMove;
        }

        private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
        {
            if (evt.customStyle.TryGetValue(draggableEnableProperty, out bool got))
            {
                enabled = got;
            }
        }

        public void DragBeginForce(PointerDownEvent evt)
        {
            if (!enabled)
            {
                return;
            }
            
            target.AddToClassList("draggable--dragging");

            if (removeClassOnDrag != null)
            {
                removeClass = target.ClassListContains(removeClassOnDrag);
                if (removeClass)
                {
                    target.RemoveFromClassList(removeClassOnDrag);
                }
            }

            lastPickingMode = target.pickingMode;
            target.pickingMode = PickingMode.Ignore;
            isDragging = true;
            target.CapturePointer(evt.pointerId);

            OnDrag?.Invoke(evt);
        }

        public void DragBeginForce(PointerDownEvent evt, Vector2 localPosition)
        {
            if (!enabled)
            {
                return;
            }
            
            target.AddToClassList("draggable--dragging");

            if (removeClassOnDrag != null)
            {
                removeClass = target.ClassListContains(removeClassOnDrag);
                if (removeClass)
                {
                    target.RemoveFromClassList(removeClassOnDrag);
                }
            }

            lastPickingMode = target.pickingMode;
            target.pickingMode = PickingMode.Ignore;
            isDragging = true;
            offset = localPosition;
            target.CapturePointer(evt.pointerId);
            OnDrag?.Invoke(evt);
        }

        private void DragBegin(PointerDownEvent evt)
        {
            if (!enabled)
            {
                return;
            }

            if (evt.button != m_Button)
            {
                return;
            }

            target.AddToClassList("draggable--dragging");

            if (removeClassOnDrag != null)
            {
                removeClass = target.ClassListContains(removeClassOnDrag);
                if (removeClass)
                {
                    target.RemoveFromClassList(removeClassOnDrag);
                }
            }

            lastPickingMode = target.pickingMode;
            target.pickingMode = PickingMode.Ignore;
            isDragging = true;
            offset = evt.localPosition;
            target.CapturePointer(evt.pointerId);

            OnDrag?.Invoke(evt);
        }

        private void DragEnd(IPointerEvent evt)
        {
            if (!isDragging)
            {
                return;
            }

            if (evt.button != m_Button)
            {
                return;
            }

            VisualElement droppable;
            bool canDrop = CanDrop(evt.position, out droppable) || !m_CheckDroppable;
            if (canDrop && droppable != null)
            {
                droppable.RemoveFromClassList("droppable--can-drop");
            }

            target.RemoveFromClassList("draggable--dragging");
            target.RemoveFromClassList("draggable--can-drop");

            lastDroppable?.RemoveFromClassList("droppable--can-drop");
            lastDroppable = null;

            target.ReleasePointer(evt.pointerId);
            target.pickingMode = lastPickingMode;
            isDragging = false;
            if (canDrop)
            {
                Drop(droppable);
            }
            else
            {
                ResetPosition();   
            }

            if (removeClassOnDrag != null && removeClass)
            {
                target.AddToClassList(removeClassOnDrag);
            }
            OnDrop?.Invoke();
        }

        private void Drop(VisualElement droppable)
        {
            var evt = DropEvent.GetPooled(this, droppable);
            evt.target = target;
            //下一帧改变class list
            target.schedule.Execute(() => evt.target.SendEvent(evt));
        }

        /** Change parent while preserving position via `transform.position`.
        Usage: While dragging-and-dropping an element, if the dropped element were
        to change its parent in the hierarchy, but preserve its position on
        screen, which can be done with `transform.position`. Then one can lerp
        that position to zero for a nice clean transition.
        Notes: The algorithm isn't difficult. It's find position wrt new parent,
        zero out the `transform.position`, add it to the parent, find position wrt
        new parent, set `transform.position` such that its screen position will be
        the same as before.
        The tricky part is when you add this element to a newParent, you can't
        query for its position (at least not in a way I could find). You have to
        wait a beat. Then whatever was necessary to update will update.
        */
        public static IVisualElementScheduledItem ChangeParent(VisualElement target, VisualElement newParent)
        {
            var position_parent = target.ChangeCoordinatesTo(newParent, Vector2.zero);
            target.RemoveFromHierarchy();
            target.transform.position = Vector3.zero;
            newParent.Add(target);
            // ChangeCoordinatesTo will not be correct unless you wait a tick. #hardwon
            // target.transform.position = position_parent - target.ChangeCoordinatesTo(newParent,
            //                                                                      Vector2.zero);
            return target.schedule.Execute(() =>
            {
                var newPosition = position_parent - target.ChangeCoordinatesTo(newParent, Vector2.zero);
                target.RemoveFromHierarchy();
                target.transform.position = newPosition;
                newParent.Add(target);
            });
        }

        /** Reset the target's position to zero.
        Note: Schedules the change so that the USS classes will be restored when
        run. (Helps when a "transitions" USS class is used.)
        */
        private void ResetPosition()
        {
            target.transform.position = Vector3.zero;
        }

        private bool CanDrop(Vector3 position, out VisualElement droppable)
        {
            //返回此位置的顶部元素。不会返回选择模式设置为 PickingMode.Ignore 的元素。
            droppable = target.panel.Pick(position);
            var element = droppable;
            //Walk up parent elements to see if any are droppables
            while (element != null && !element.ClassListContains(droppableId))
            {
                element = element.parent;
            }

            if (element != null)
            {
                droppable = element;
                return true;
            }

            return false;
        }
        
        private void PointerMove(PointerMoveEvent evt)
        {
            if (!isDragging)
            {
                return;
            }

            if (!enabled)
            {
                DragEnd(evt);
                return;
            }

            Vector3 delta = evt.localPosition - offset;
            OnMove?.Invoke(delta);

            if (CanDrop(evt.localPosition, out var droppable))
            {
                target.AddToClassList("draggable--can-drop");
                droppable.AddToClassList("droppable--can-drop");
                if (lastDroppable != droppable)
                {
                    lastDroppable?.RemoveFromClassList("droppable--can-drop");
                }

                lastDroppable = droppable;
            }
            else
            {
                target.RemoveFromClassList("draggable--can-drop");
                lastDroppable?.RemoveFromClassList("droppable--can-drop");
                lastDroppable = null;
            }
        }
    }

    public class DropEvent: EventBase<DropEvent>
    {
        public DragManipulator dragger { get; protected set; }
        public VisualElement droppable { get; protected set; }

        protected override void Init()
        {
            base.Init();
            LocalInit();
        }

        private void LocalInit()
        {
            bubbles = true;
            tricklesDown = false;
        }

        public static DropEvent GetPooled(DragManipulator dragger, VisualElement dropable)
        {
            DropEvent pooled = EventBase<DropEvent>.GetPooled();
            pooled.dragger = dragger;
            pooled.droppable = dropable;
            return pooled;
        }

        public DropEvent() => LocalInit();
    }
}