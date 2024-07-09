using System;
using System.Collections.Generic;
using ET;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineTrackView: VisualElement, ISelectable
    {
        public new class UxmlFactory: UxmlFactory<TimelineTrackView, UxmlTraits>
        {
        }

        public TimelineEditorWindow EditorWindow => FieldView.EditorWindow;
        protected readonly DropdownMenuHandler m_MenuHandler;
        
        private readonly DoubleMap<BBClip, TimelineClipView> ClipViewMap = new();
        protected readonly List<MarkerView> markerViews = new();
        
        public RuntimeTrack RuntimeTrack;
        private BBTrack BBTrack => RuntimeTrack.Track;

        public TimelineTrackView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineTrackView");
            visualTree.CloneTree(this);
            AddToClassList("timelineTrack");

            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerMoveEvent>(OnPointerMove);
            RegisterCallback<PointerOutEvent>(OnPointerOut);

            m_MenuHandler = new DropdownMenuHandler(MenuBuilder);
        }

        public virtual void Init(RuntimeTrack track)
        {
            RuntimeTrack = track;
            int index = EditorWindow.RuntimePlayable.RuntimeTracks.IndexOf(track);
            transform.position = new Vector3(0, index * 40, 0);

            //Init ClipView
            ClipViewMap.Clear();
            foreach (BBClip clip in RuntimeTrack.Track.Clips)
            {
                TimelineClipView clipView = Activator.CreateInstance(RuntimeTrack.Track.ClipViewType) as TimelineClipView;
                clipView.SelectionContainer = FieldView;
                clipView.Init(clip, this);

                Add(clipView);
                ClipViewMap.Add(clip, clipView);
                FieldView.SelectionElements.Add(clipView);
            }
        }

        public virtual void Refresh()
        {
            foreach (TimelineClipView clipViewValue in ClipViewMap.Values)
            {
                clipViewValue.Refresh();
            }
        }

        #region Selectable

        private bool m_Selected;
        public ISelection SelectionContainer { get; set; }
        public TimelineFieldView FieldView => SelectionContainer as TimelineFieldView;

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
            m_Selected = true;
            AddToClassList("selected");
            BringToFront();
        }

        public void UnSelect()
        {
            m_Selected = false;
            RemoveFromClassList("selected");
        }

        #endregion

        protected virtual void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Add Clip",
                _ => { EditorWindow.ApplyModify(() => { BBTrack.AddClip(FieldView.GetCurrentTimeLocator()); }, "Add Clip"); });
        }

        protected virtual void OnPointerDown(PointerDownEvent evt)
        {
            //当前选中了Clip
            foreach (TimelineClipView v in ClipViewMap.Values)
            {
                if (!v.InMiddle(evt.position)) continue;

                v.OnPointerDown(evt);
                evt.StopImmediatePropagation();
                return;
            }

            if (evt.button == 1)
            {
                m_MenuHandler.ShowMenu(evt);
                evt.StopImmediatePropagation();
            }
        }

        protected virtual void OnPointerMove(PointerMoveEvent evt)
        {
            foreach (TimelineClipView clipViewValue in ClipViewMap.Values)
            {
                clipViewValue.OnHover(false);
                if (clipViewValue.InMiddle(evt.position))
                {
                    clipViewValue.OnHover(true);
                    evt.StopImmediatePropagation();
                }
            }
        }

        protected virtual void OnPointerOut(PointerOutEvent evt)
        {
            foreach (TimelineClipView clipViewValue in ClipViewMap.Values)
            {
                clipViewValue.OnHover(false);
            }
        }

        #region Move Marker

        private int startMoveMarkerFrame;

        public void MarkerStartMove(MarkerView markerView)
        {
            startMoveMarkerFrame = markerView.keyframeBase.frame;
        }

        public void MoveMarkers(float deltaPosition)
        {
            int startFrame = int.MaxValue;
            List<MarkerView> moveMarkers = new List<MarkerView>();
            
            //1. 获得选中的markerView
            foreach (ISelectable selectable in FieldView.Selections)
            {
                if (selectable is MarkerView markerView)
                {
                    moveMarkers.Add(markerView);
                    if (markerView.keyframeBase.frame < startFrame)
                    {
                        startFrame = markerView.keyframeBase.frame;
                    }
                }
            }

            if (moveMarkers.Count == 0)
            {
                return;
            }
            
            //2. Move markerView
            int targetStartFrame = FieldView.GetClosestFrame(FieldView.FramePosMap[startFrame] + deltaPosition);
            int deltaFrame = targetStartFrame - startFrame;
            foreach (MarkerView marker in moveMarkers)
            {
                marker.Move(deltaFrame);
            }

            //3. Resize frameMap
            int maxFrame = int.MinValue;
            foreach (MarkerView marker in moveMarkers)
            {
                if (marker.keyframeBase.frame >= maxFrame)
                {
                    maxFrame = marker.keyframeBase.frame;
                }
            }

            FieldView.ResizeTimeField(maxFrame);

            //4. check overlap
            foreach (MarkerView marker in moveMarkers)
            {
                marker.InValid = GetMarkerMoveValid(marker);
            }

            Refresh();
            FieldView.DrawFrameLine(startFrame);
        }

        private bool GetMarkerMoveValid(MarkerView markerView)
        {
            foreach (MarkerView view in markerViews)
            {
                if (view == markerView)
                {
                    continue;
                }

                if (view.keyframeBase.frame == markerView.keyframeBase.frame)
                {
                    return false;
                }
            }

            return true;
        }

        public void ApplyMarkerMove()
        {
            int startFrame = int.MaxValue;
            bool InValid = true;

            List<HitboxMarkerView> moveMarkers = new();
            foreach (ISelectable selection in FieldView.Selections)
            {
                if (selection is not HitboxMarkerView markerView) continue;

                //override with other marker
                if (!markerView.InValid)
                {
                    InValid = false;
                }

                if (markerView.keyframe.frame <= startFrame)
                {
                    startFrame = markerView.keyframe.frame;
                }

                moveMarkers.Add(markerView);
            }

            int deltaFrame = startFrame - startMoveMarkerFrame;

            if (deltaFrame != 0)
            {
                //Reset position
                foreach (HitboxMarkerView markerView in moveMarkers)
                {
                    markerView.ResetMove(deltaFrame);
                }

                if (InValid)
                {
                    EditorWindow.ApplyModify(() =>
                    {
                        foreach (HitboxMarkerView markerView in moveMarkers)
                        {
                            markerView.Move(deltaFrame);
                        }
                    }, "Move hitbox markers");
                }
            }

            Refresh();
            FieldView.DrawFrameLine();
        }

        #endregion

        // private class DragAndDropManipulator: PointerManipulator
        // {
        //     private Label dropLabel;
        //     public Func<bool> DragValid;
        //     public Action<UnityEngine.Object, Vector2> DragPerform;
        //
        //     public DragAndDropManipulator(VisualElement root)
        //     {
        //         //The target of the manipulator , the object to which to register all callback,is the drop areas
        //         target = root.Q<VisualElement>(className: "drop-area");
        //         dropLabel = root.Q<Label>(className: "drop-area__label");
        //     }
        //
        //     protected override void RegisterCallbacksOnTarget()
        //     {
        //         //Register callback for various stages in the drag proess
        //         target.RegisterCallback<DragEnterEvent>(OnDragEnter);
        //         target.RegisterCallback<DragLeaveEvent>(OnDragLeave);
        //         target.RegisterCallback<DragUpdatedEvent>(OnDragUpdate);
        //         target.RegisterCallback<DragPerformEvent>(OnDragPerform);
        //     }
        //
        //     protected override void UnregisterCallbacksFromTarget()
        //     {
        //         target.UnregisterCallback<DragEnterEvent>(OnDragEnter);
        //         target.UnregisterCallback<DragLeaveEvent>(OnDragLeave);
        //         target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdate);
        //         target.UnregisterCallback<DragPerformEvent>(OnDragPerform);
        //     }
        //
        //     private void OnDragEnter(DragEnterEvent _)
        //     {
        //         //Get the name of the object the user is dragging
        //         // var draggerName = string.Empty;
        //         // if (DragAndDrop.objectReferences.Length > 0)
        //         // {
        //         //     draggerName = DragAndDrop.objectReferences[0].name;
        //         // }
        //
        //         //Change the appearance of the drop area if the user is dragging
        //         target.AddToClassList("drop-area--dropping");
        //     }
        //
        //     //this method run if a user makes the pointer leave the bounds of the target while a drag is in progress
        //     private void OnDragLeave(DragLeaveEvent _)
        //     {
        //         target.RemoveFromClassList("drop-area--dropping");
        //     }
        //
        //     //this method runs every frame while a drag is in progress
        //     private void OnDragUpdate(DragUpdatedEvent _)
        //     {
        //         DragAndDrop.visualMode = DragValid()? DragAndDropVisualMode.Generic : DragAndDropVisualMode.None;
        //     }
        //
        //     //this method run when a user drops a dragged object onto the target
        //     private void OnDragPerform(DragPerformEvent _)
        //     {
        //         // var draggedName = string.Empty;
        //         if (DragAndDrop.objectReferences.Length > 0)
        //         {
        //             // draggedName = DragAndDrop.objectReferences[0].name;
        //             DragPerform?.Invoke(DragAndDrop.objectReferences[0], _.localMousePosition);
        //         }
        //
        //         //Visually update target to indicate that it now stores an asset
        //         //droplabel.text = $"Containing '{draggedName}'";
        //         target.RemoveFromClassList("drop-area-dropping");
        //     }
        // }
    }
}