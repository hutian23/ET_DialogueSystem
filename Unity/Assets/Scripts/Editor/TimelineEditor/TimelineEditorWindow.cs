using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineEditorWindow: EditorWindow, ISelection
    {
        // private VisualElement m_Top;
        // private VisualElement m_LeftPanel;
        protected VisualElement m_TrackHierarchy;
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
        public TimelinePlayer TimelinePlayer { get; private set; }

        public BBTimeline BBTimeline => TimelinePlayer.RuntimePlayable.timeline;
        public RuntimePlayable RuntimePlayable => TimelinePlayer.RuntimePlayable;
        private SerializedObject SerializedTimeline => TimelinePlayer.RuntimePlayable.timeline.SerializedTimeline;

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineEditorWindow");
            visualTree.CloneTree(root);
            root.AddToClassList("timelineEditorWindow");

            // m_Top = root.Q("top");

            m_PlayButton = root.Q<Button>("play-button");
            m_PlayButton.clicked += () => { m_TimelineField.PlayTimelineCor(); };

            m_PauseButton = root.Q<Button>("pause-button");
            m_PauseButton.clicked += () => { m_TimelineField.StopPlayTimelineCor(); };

            m_LoopPlayButton = root.Q<Button>("loop-button");
            m_LoopPlayButton.clicked += () => { m_TimelineField.LoopPlayTimelineCor(); };

            fieldScaleBar = root.Q<SliderInt>("field-scale-bar");

            // m_LeftPanel = root.Q("left-panel");
            m_TrackHierarchy = root.Q("track-hierachy");
            m_Toolbar = root.Q("tool-bar");

            //Scroll trackView
            TrackHandleContainer = root.Q<ScrollView>("track-handle-container");
            TrackHandleContainer.focusable = true;
            TrackHandleContainer.verticalScrollerVisibility = ScrollerVisibility.Hidden;
            TrackHandleContainer.RegisterCallback<WheelEvent>(_ =>
            {
                foreach (VisualElement child in TrackHandleContainer.Children())
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
                        return;
                    }
                }
            });

            //Add Track
            m_AddTrackButton = root.Q("add-track-button");
            m_AddTrackButton.AddManipulator(new DropdownMenuManipulator((menu) =>
            {
                foreach (var type in BBTimelineEditorUtility.BBTrackTypeDic)
                {
                    menu.AppendAction(type.Key, _ => { ApplyModify(() => { RuntimePlayable.AddTrack(type.Value); }, "Add Track"); });
                }
            }, MouseButton.LeftMouse));

            //Select Timeline
            m_select_timeline_Button = root.Q<Button>("select-timeline-button");
            
            DropdownMenuHandler selectMenuHandler = new(menu =>
            {
                foreach (BBTimeline _timeline in TimelinePlayer.PlayableGraph.timelineDict.Values)
                {
                    string actionName = $"{_timeline.timelineName}";
                    menu.AppendAction(actionName, _ => { OpenWindow(TimelinePlayer, _timeline); }, TimelinePlayer.RuntimePlayable.timeline == _timeline? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);
                }
            });
            m_select_timeline_Button.clicked += () => { selectMenuHandler.ShowMenu(m_select_timeline_Button); };
            m_select_timeline_label = root.Q<Label>("select-timeline-label");
            m_TimelineField = root.Q<TimelineFieldView>();
            m_TimelineField.EditorWindow = this;

            fieldScaleBar = root.Q<SliderInt>("field-scale-bar");
            fieldScaleBar.RegisterValueChangedCallback(m_TimelineField.SliderUpdate);

            Undo.undoRedoEvent += OnUndoRedoEvent;
        }

        private void OnDestroy()
        {
            Undo.undoRedoEvent -= OnUndoRedoEvent;
            Dispose();
            TimelinePlayer.Dispose();
        }

        public void ApplyModify(Action action, string _name, bool rebind = true)
        {
            Undo.RegisterCompleteObjectUndo(BBTimeline, $"Timeline: {_name}");
            SerializedTimeline.Update();
            action?.Invoke();

            if (rebind) TimelinePlayer.RebindCallback?.Invoke();
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
        }

        private void OnUndoRedoEvent(in UndoRedoInfo info)
        {
            if (info.undoName.Split(':')[0] == "Timeline")
            {
                TimelinePlayer.RebindCallback?.Invoke();
            }
        }

        private void UpdateBindState()
        {
            bool binding = (TimelinePlayer != null);
            m_PlayButton.SetEnabled(binding);
            m_PauseButton.SetEnabled(binding);
            fieldScaleBar.SetEnabled(binding);
            m_TimelineField.SetEnabled(binding);
            m_select_timeline_label.text = $"{TimelinePlayer.RuntimePlayable.timeline.timelineName}";
            
            RuntimePlayable.timeline.UpdateSerializeTimeline();
            TimelinePlayer.RebindCallback -= PopulateView; 
            TimelinePlayer.RebindCallback += PopulateView;
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
            TimelineEditorWindow window = GetWindow<TimelineEditorWindow>();
            window.Dispose();
            window.TimelinePlayer = timelinePlayer;
            window.TimelinePlayer.Dispose();
            window.TimelinePlayer.Init(timeline);
            window.PopulateView();
        }
    }
}