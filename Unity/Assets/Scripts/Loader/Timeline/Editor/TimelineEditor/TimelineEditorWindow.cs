using System;
using System.Collections.Generic;
using ET;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineEditorWindow: EditorWindow, ISelection
    {
        private VisualElement m_Top;
        private VisualElement m_LeftPanel;
        protected VisualElement m_TrackHierachy;
        protected VisualElement m_Toolbar;
        public ScrollView TrackHandleContainer;
        private VisualElement m_AddTrackButton;

        private SliderInt fieldScaleBar;
        private Button m_select_timeline_Button;
        private Button m_PlayButton;
        private Button m_PauseButton;
        private Button m_LoopPlayButton;
        private Label m_select_timeline_label;
        private TimelineFieldView m_TimelineField;
        // public IntegerField m_currentFrameField;
        // public TextField m_currentMarkerField;
        public TimelinePlayer TimelinePlayer { get; private set; }

        public BBTimeline BBTimeline => TimelinePlayer.RuntimeimePlayable.Timeline;
        public RuntimePlayable RuntimePlayable => TimelinePlayer.RuntimeimePlayable;
        private SerializedObject SerializedTimeline => TimelinePlayer.RuntimeimePlayable.Timeline.SerializedTimeline;

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineEditorWindow");
            visualTree.CloneTree(root);
            root.AddToClassList("timelineEditorWindow");

            m_Top = root.Q("top");

            m_PlayButton = root.Q<Button>("play-button");
            m_PlayButton.clicked += () => { m_TimelineField.PlayTimelineCor(); };

            m_PauseButton = root.Q<Button>("pause-button");
            m_PauseButton.clicked += () => { m_TimelineField.StopPlayTimelineCor(); };

            m_LoopPlayButton = root.Q<Button>("loop-button");
            m_LoopPlayButton.clicked += () => { m_TimelineField.LoopPlayTimelineCor(); };

            fieldScaleBar = root.Q<SliderInt>("field-scale-bar");

            m_LeftPanel = root.Q("left-panel");
            m_TrackHierachy = root.Q("track-hierachy");
            m_Toolbar = root.Q("tool-bar");

            //Scroll trackView
            TrackHandleContainer = root.Q<ScrollView>("track-handle-container");
            TrackHandleContainer.focusable = true;
            TrackHandleContainer.verticalScrollerVisibility = ScrollerVisibility.Hidden;
            TrackHandleContainer.RegisterCallback<WheelEvent>(_ =>
            {
                foreach (var child in TrackHandleContainer.Children())
                {
                    if (child is not TimelineTrackHandle) continue;
                    ScrollView trackScroll = root.Q<ScrollView>("track-scroll");
                    trackScroll.scrollOffset = new Vector2(trackScroll.scrollOffset.x, TrackHandleContainer.scrollOffset.y);
                    return;
                }
            });
            TrackHandleContainer.RegisterCallback<PointerDownEvent>((e) =>
            {
                foreach (TimelineTrackHandle timelineTrackHandle in TrackHandleContainer.Query<TimelineTrackHandle>().ToList())
                {
                    //选中trackHandle
                    if (timelineTrackHandle.worldBound.Contains(e.position))
                    {
                        timelineTrackHandle.OnPointerDown(e);
                        e.StopImmediatePropagation();
                        return;
                    }
                }

                if (e.button == 0)
                {
                    m_TimelineField.ClearSelection();
                    e.StopImmediatePropagation();
                }
            });

            //Add Track
            m_AddTrackButton = root.Q("add-track-button");
            m_AddTrackButton.AddManipulator(new DropdownMenuManipulator((menu) =>
            {
                menu.AppendAction("Marker", _ =>
                {
                    ApplyModify(() =>
                    {
                        MarkerInfo info = new() { frame = m_TimelineField.GetCurrentTimeLocator(), markerName = "TimelineMarker" };
                        BBTimeline.Marks.Add(info);
                    }, "Add Marker");
                });
                menu.AppendSeparator();
                foreach (var type in BBTimelineEditorUtility.BBTrackTypeDic)
                {
                    menu.AppendAction(type.Key, _ => { ApplyModify(() => { RuntimePlayable.AddTrack(type.Value); }, "Add Track"); });
                }
            }, MouseButton.LeftMouse));

            //Select Timeline
            m_select_timeline_Button = root.Q<Button>("select-timeline-button");
            DropdownMenuHandler selectMenuHandler = new(menu =>
            {
                foreach (BBTimeline _timeline in TimelinePlayer.BBPlayable.GetTimelines())
                {
                    string actionName = $"{_timeline.timelineName}";
                    menu.AppendAction(actionName, _ => { TimelinePlayer.OpenWindow(_timeline); },
                        TimelinePlayer.CurrentTimeline == _timeline? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);
                }
            });
            m_select_timeline_Button.clicked += () => { selectMenuHandler.ShowMenu(m_select_timeline_Button); };

            m_select_timeline_label = root.Q<Label>("select-timeline-label");

            m_TimelineField = root.Q<TimelineFieldView>();
            m_TimelineField.EditorWindow = this;

            fieldScaleBar = root.Q<SliderInt>("field-scale-bar");
            fieldScaleBar.RegisterValueChangedCallback(m_TimelineField.SliderUpdate);

            // m_currentFrameField = root.Q<IntegerField>("current-frame-field");
            // m_currentFrameField.RegisterCallback<BlurEvent>(_ =>
            // {
            //     if (m_currentFrameField.value >= 500) m_currentFrameField.SetValueWithoutNotify(500);
            //     m_TimelineField.CurrentFrameFieldUpdate(m_currentFrameField.value);
            // });

            // m_currentMarkerField = root.Q<TextField>("current-marker-field");
            // m_currentMarkerField.RegisterCallback<BlurEvent>(_ =>
            // {
            //     foreach (var mark in BBTimeline.Marks)
            //     {
            //         if (!mark.markerName.Equals(m_currentMarkerField.value)) continue;
            //         int frame = mark.frame;
            //         m_TimelineField.CurrentFrameFieldUpdate(frame);
            //     }
            // });

            Undo.undoRedoEvent += OnUndoRedoEvent;
        }

        private void OnDestroy()
        {
            Undo.undoRedoEvent -= OnUndoRedoEvent;
            Dispose();
            RuntimePlayable?.Dispose();
        }

        public void ApplyModify(Action action, string _name, bool rebind = true)
        {
            Undo.RegisterCompleteObjectUndo(BBTimeline, $"Timeline: {_name}");
            SerializedTimeline.Update();
            action?.Invoke();

            if (rebind) RuntimePlayable.RebindCallback?.Invoke();
            EditorUtility.SetDirty(BBTimeline);
        }

        public void ApplyModifyWithoutButtonUndo(Action action, string _name, bool rebind = true)
        {
            //不希望按钮事件添加到undo中
            Undo.IncrementCurrentGroup();
            int undoGroup = Undo.GetCurrentGroup();
            ApplyModify(action, _name, rebind);
            Undo.CollapseUndoOperations(undoGroup);
        }

        private void Dispose()
        {
            m_TimelineField.Dispose();
        }

        private void PopulateView()
        {
            TrackHandleContainer.Clear();
            TrackHandleContainer.ForceScrollViewUpdate();
            m_Elements.Clear();
            m_Selections.Clear();

            UpdateBindState();
            m_TimelineField.PopulateView();
            UpdateSelectTimeline();
        }

        private void OnUndoRedoEvent(in UndoRedoInfo info)
        {
            if (info.undoName.Split(':')[0] == "Timeline" && RuntimePlayable != null)
            {
                RuntimePlayable.RebindCallback?.Invoke();
            }
        }

        private void UpdateBindState()
        {
            bool binding = (TimelinePlayer != null);
            m_PlayButton.SetEnabled(binding);
            m_PauseButton.SetEnabled(binding);
            fieldScaleBar.SetEnabled(binding);
            m_TimelineField.SetEnabled(binding);

            RuntimePlayable.Timeline.UpdateSerializeTimeline();
            RuntimePlayable.RebindCallback -= PopulateView;
            RuntimePlayable.RebindCallback += PopulateView;
        }

        #region Selection

        public VisualElement ContentContainer => TrackHandleContainer;

        private readonly List<ISelectable> m_Elements = new();
        public List<ISelectable> SelectionElements => m_Elements;

        private readonly List<ISelectable> m_Selections = new();
        public List<ISelectable> Selections => m_Selections;

        public void AddToSelection(ISelectable selectable)
        {
            m_Selections.Add(selectable);
            selectable.Select();
        }

        public void RemoveFromSelection(ISelectable selectable)
        {
            m_Selections.ForEach(i => i.UnSelect());
            Selections.Clear();
        }

        public void ClearSelection()
        {
            m_Selections.ForEach(i => i.UnSelect());
            Selections.Clear();
        }

        #endregion

        public static void OpenWindow(TimelinePlayer timelinePlayer, BBTimeline timeline)
        {
            //Stop runtime behavior
            EventSystem.Instance?.Invoke(new EditTimelineCallback() { instanceId = timelinePlayer.instanceId });

            TimelineEditorWindow window = GetWindow<TimelineEditorWindow>();
            window.Dispose();
            window.TimelinePlayer = timelinePlayer;
            window.TimelinePlayer.Dispose();
            window.TimelinePlayer.Init(timeline);
            window.PopulateView();
            
        }

        #region Select Timeline

        private void UpdateSelectTimeline()
        {
            m_select_timeline_label.text = TimelinePlayer.CurrentTimeline.timelineName;
        }

        #endregion
    }
}