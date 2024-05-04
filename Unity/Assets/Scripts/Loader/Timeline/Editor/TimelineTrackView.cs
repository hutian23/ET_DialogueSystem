using System;
using System.Collections.Generic;
using ET;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineTrackView: VisualElement, ISelectable
    {
        public new class UxmlFactory: UxmlFactory<TimelineTrackView, UxmlTraits>
        {
        }

        private bool m_Selected;
        public ISelection SelectionContainer { get; set; }
        public TimelineFieldView FieldView => SelectionContainer as TimelineFieldView;
        public TimelineEditorWindow EditorWindow => FieldView.EditorWindow;
        private Timeline Timeline => EditorWindow.Timeline;
        public Track Track { get; private set; }
        public DoubleMap<Clip, TimelineClipView> ClipViewMap { get; private set; }
        public List<TimelineClipView> ClipViews { get; private set; }

        public Action OnSelected;
        public Action OnUnSelected;

        private readonly DropdownMenuHandler m_MenuHandler;
        private Vector2 m_localMousePosition;

        public RuntimeTrack RuntimeTrack;
        private BBTrack BBTrack => RuntimeTrack.Track;
        public DoubleMap<BBClip, TimelineClipView> BBClipViewMap = new();

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

        public void Init(RuntimeTrack track)
        {
            RuntimeTrack = track;
            int index = EditorWindow.RuntimePlayable.RuntimeTracks.IndexOf(track);
            transform.position = new Vector3(0, index * 40, 0);

            //Init ClipView
            foreach (var clip in RuntimeTrack.Track.Clips)
            {
                TimelineClipView clipView = new();
                clipView.SelectionContainer = FieldView;
                clipView.Init(clip, this);
                Add(clipView);

                BBClipViewMap.Add(clip, clipView);
            }
        }

        public void Init(Track track)
        {
            Track = track;
            // Track.OnUpdateMix = Refreh;
            // Track.OnMutedStateChanged = OnMutedStateChanged;
            ClipViewMap = new DoubleMap<Clip, TimelineClipView>();
            ClipViews = new List<TimelineClipView>();
            foreach (Clip clip in track.Clips)
            {
                TimelineClipView clipView = new();
                clipView.SelectionContainer = FieldView;
                clipView.Init(clip, this);

                Add(clipView);
                FieldView.Elements.Add(clipView); // IsSelectable
                ClipViewMap.Add(clip, clipView);
                ClipViews.Add(clipView);
            }

            DragAndDropManipulator dragAndDropManipulator = new(this);
            dragAndDropManipulator.DragValid = Track.DragValid;
            dragAndDropManipulator.DragPerform += (_, _) =>
            {
                // int startFrame = FieldView.GetClosestFloorFrame(e2.x);
                // if (Track.Clips.Find(i => i.StartFrame == startFrame) == null)
                // {
                //     Timeline.ApplyModify(() => { FieldView.AddClip(e1, Track, startFrame); }, "Add Clip");
                // }
                Timeline.ApplyModify(() => { FieldView.AddClip(track); }, "AddClip");
            };
            this.AddManipulator(dragAndDropManipulator);
            transform.position = new Vector3(0, Timeline.Tracks.IndexOf(track) * 40, 0);

            OnMutedStateChanged();
        }

        public void Refreh()
        {
            foreach (TimelineClipView clipViewValue in ClipViewMap.Values)
            {
                clipViewValue.Refresh();
            }
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
            m_Selected = true;
            AddToClassList("selected");
            BringToFront();
            OnSelected?.Invoke();
        }

        public void UnSelect()
        {
            m_Selected = false;
            RemoveFromClassList("selected");

            OnUnSelected?.Invoke();
        }

        #endregion

        private void MenuBuilder(DropdownMenu menu)
        {
            // menu.AppendAction("Add Clip", _ => { Timeline.ApplyModify(() => { FieldView.AddClip(Track); }, "Add Clip"); });
            // menu.AppendAction("Remove Track", _ => { Timeline.ApplyModify(() => { Timeline.RemoveTrack(Track); }, "Remove Track"); });
            // menu.AppendAction("Open Script", _ => { Track.OpenTrackScript(); });
            menu.AppendAction("Add Clip", _ =>
            {
                //TODO AddClip
                EditorWindow.ApplyModify(() => { BBTrack.AddClip(FieldView.GetCurrentTimeLocator()); }, "Add Clip");
            });
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            //当前选中了Clip
            foreach (TimelineClipView v in BBClipViewMap.Values)
            {
                if(!v.InMiddle(evt.position)) continue;
                
                v.OnPointerDown(evt);
                evt.StopImmediatePropagation();
                return;
            }
            
            if (evt.button == 1)
            {
                m_localMousePosition = evt.localPosition;
                m_MenuHandler.ShowMenu(evt);
                evt.StopImmediatePropagation();
            }
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            // foreach (TimelineClipView clipViewValue in ClipViewMap.Values)
            // {
            //     clipViewValue.OnHover(false);
            //     if (clipViewValue.InMiddle(evt.position))
            //     {
            //         clipViewValue.OnHover(true);
            //         evt.StopImmediatePropagation();
            //     }
            // }
        }

        private void OnPointerOut(PointerOutEvent evt)
        {
            // foreach (TimelineClipView clipViewValue in ClipViewMap.Values)
            // {
            //     clipViewValue.OnHover(false);
            // }
        }

        private void OnMutedStateChanged()
        {
            SetEnabled(!Track.PersistentMuted && !Track.RuntimeMuted);
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
                // var draggerName = string.Empty;
                // if (DragAndDrop.objectReferences.Length > 0)
                // {
                //     draggerName = DragAndDrop.objectReferences[0].name;
                // }

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
                DragAndDrop.visualMode = DragValid()? DragAndDropVisualMode.Generic : DragAndDropVisualMode.None;
            }

            //this method run when a user drops a dragged object onto the target
            private void OnDragPerform(DragPerformEvent _)
            {
                // var draggedName = string.Empty;
                if (DragAndDrop.objectReferences.Length > 0)
                {
                    // draggedName = DragAndDrop.objectReferences[0].name;
                    DragPerform?.Invoke(DragAndDrop.objectReferences[0], _.localMousePosition);
                }

                //Visually update target to indicate that it now stores an asset
                //droplabel.text = $"Containing '{draggedName}'";
                target.RemoveFromClassList("drop-area-dropping");
            }
        }
    }
}