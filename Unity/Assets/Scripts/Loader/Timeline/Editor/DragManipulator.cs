using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class DragManipulator: IManipulator
    {
        public VisualElement _target;

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
                    // _target.UnregisterCallback<PointerDownEvent>();
                }
            }
        }

        protected static readonly CustomStyleProperty<bool> draggableEnableProperty = new CustomStyleProperty<bool>("--draggable-enabled");
        protected Vector3 offset;
        private bool isDragging;
        private VisualElement lastDroppable = null;
        private string _droppabled = "droppable";

        public string droppableId
        {
            get => _droppabled;
            set => _droppabled = value;
        }

        /** This manipulator can be disabled. */
        public bool enabled { get; set; } = true;

        private PickingMode lastPickingMode;
        private string _removeClassOnDrag;

        public string removeClassOnDrag
        {
            get => _removeClassOnDrag;
            set => _removeClassOnDrag = value;
        }

        private bool removeClass = false;
        protected bool m_CheckDroppable;
        protected int m_Button;
        public Action<PointerDownEvent> OnDrag;
        public Action OnDrop;
        public Action<Vector2> OnMove;

        public DragManipulator(bool checkDroppable, int button = 0)
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
            offset = (Vector3)localPosition;
            target.CapturePointer(evt.pointerId);
            OnDrag?.Invoke(evt);
        }

        public void DragBegin(PointerDownEvent evt)
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

        public void DragEnd(IPointerEvent evt)
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
            // bool canDrop = CanDro
        }

        protected virtual bool CanDrop(Vector3 position, out VisualElement droppable)
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

            Vector3 delta = evt.localPosition - (Vector3)offset;
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