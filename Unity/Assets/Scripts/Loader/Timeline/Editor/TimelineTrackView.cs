using System;
using System.Collections.Generic;
using ET;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineTrackView : VisualElement,ISelectable
    {
        public new class UxmlFactory : UxmlFactory<TimelineTrackView,UxmlTraits> {}
        protected bool m_Selected;
        public ISelection SelectionContainer { get; set; }
        public TimelineFieldView FieldView => SelectionContainer as TimelineFieldView;
        public TimelineEditorWindow EditorWindow;
        public Timeline Timeline;
        public Track Track { get; private set; }
        public DoubleMap<Clip,TimelineClipView> ClipViewMap { get; private set; }
        public List<TimelineClipView> ClipViews { get; set; }
        
        public Action OnSelected;
        public Action OnUnSelected;

        private DropdownMenuHandler m_MenuHandler;
        private Vector2 m_localMousePosition;

        public TimelineTrackView()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("VisualTree/TimelineTrackView");
            visualTree.CloneTree(this);
            AddToClassList("timelineTrack");
            
            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerMoveEvent>(OnPointerMove);
            RegisterCallback<PointerOutEvent>(OnPointerOut);

            m_MenuHandler = new DropdownMenuHandler(MenuBuilder);
        }

        public void Init(Track track)
        {
            
        }

        public void Refreh()
        {
            
        }

        #region Selectable

        public override bool Overlaps(Rect rectangle)
        {
            return false;
        }
        
        public bool IsSelectable()
        {
            return false;
        }

        public bool IsSelected()
        {
            return m_Selected;
        }

        public void Select()
        {
            
        }

        public void UnSelect()
        {
            
        }

        #endregion

        private void MenuBuilder(DropdownMenu menu)
        {
            
        }
        
        private void OnPointerDown(PointerDownEvent evt)
        {
            foreach (var v in ClipViewMap.Values)
            {
                // if(v.inm)
            }
        }
        
        private void OnPointerMove(PointerMoveEvent evt)
        {
            
        }

        private void OnPointerOut(PointerOutEvent evt)
        {
            
        }

        private void OnMutedStateChanged()
        {
            
        }

        private class DragAndDropManipulator: PointerManipulator
        {
            private Label dropLabel;
            public Func<bool> DragValid;
            public Action<UnityEngine.Object, Vector2> DragPerform;

            public DragAndDropManipulator(VisualElement root)
            {
                //The target of the manipulator , the object to which to register all callback,is the drop areas
                target = root.Q<VisualElement>(className: "drop-area");
                dropLabel = root.Q<Label>(className: "drop-area__label");
            }
                    
            protected override void RegisterCallbacksOnTarget()
            {
                //Register callback for various stages in the drag proess
                target.RegisterCallback<DragEnterEvent>(OnDragEnter);
                target.RegisterCallback<DragLeaveEvent>(OnDragLeave);
                target.RegisterCallback<DragUpdatedEvent>(OnDragUpdate);
                target.RegisterCallback<DragPerformEvent>(OnDragPerform);
            }

            protected override void UnregisterCallbacksFromTarget()
            {
                target.UnregisterCallback<DragEnterEvent>(OnDragEnter);
                target.UnregisterCallback<DragLeaveEvent>(OnDragLeave);
                target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdate);
                target.UnregisterCallback<DragPerformEvent>(OnDragPerform);
            }

            private void OnDragEnter(DragEnterEvent _)
            {
                //Get the name of the object the user is dragging
                var draggerName = string.Empty;
                if (DragAndDrop.objectReferences.Length > 0)
                {
                    draggerName = DragAndDrop.objectReferences[0].name;
                }
                
                //Change the appearance of the drop area if the user is dragging
                target.AddToClassList("drop-area--dropping");
            }
            
            //this method run if a user makes the pointer leave the bounds of the target while a drag is in progress
            private void OnDragLeave(DragLeaveEvent _)
            {
                target.RemoveFromClassList("drop-area--dropping");
            }

            //this method runs every frame while a drag is in progress
            private void OnDragUpdate(DragUpdatedEvent _)
            {
                if (DragValid())
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                }
                else
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.None;
                }
            }
            
            //this method run when a user drops a dragged object onto the target
            private void OnDragPerform(DragPerformEvent _)
            {
                var draggedName = string.Empty;
                if (DragAndDrop.objectReferences.Length > 0)
                {
                    draggedName = DragAndDrop.objectReferences[0].name;
                    DragPerform?.Invoke(DragAndDrop.objectReferences[0],_.localMousePosition);
                }
                //Visually update target to indicate that it now stores an asset
                //droplabel.text = $"Containing '{draggedName}'";
                target.RemoveFromClassList("drop-area-dropping");
            }
        }
    }
}