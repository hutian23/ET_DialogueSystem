using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
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
        private Button m_PlayButton;
        private Button m_PauseButton;

        private TimelineFieldView m_TimelineField;
        public Timeline Timeline { get; private set; }
        public TimelinePlayer TimelinePlayer { get; set; }

        public BBTimeline BBTimeline => TimelinePlayer.RuntimeimePlayable.Timeline;
        public RuntimePlayable RuntimePlayable => TimelinePlayer.RuntimeimePlayable;
        public SerializedObject SerializedTimeline => TimelinePlayer.RuntimeimePlayable.Timeline.SerializedTimeline;

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineEditorWindow");
            visualTree.CloneTree(root);
            root.AddToClassList("timelineEditorWindow");

            m_Top = root.Q("top");
            //运行时不可用
            //TODO 热重载之后改成运行时支持修改

            m_PlayButton = root.Q<Button>("play-button");
            // m_PlayButton.clicked += () => { Timeline.TimelinePlayer.IsPlaying = true; };
            //
            m_PauseButton = root.Q<Button>("pause-button");
            // // m_PauseButton.clicked += () => { Timeline.TimelinePlayer.IsPlaying = false; };

            fieldScaleBar = root.Q<SliderInt>("field-scale-bar");

            m_LeftPanel = root.Q("left-panel");
            m_TrackHierachy = root.Q("track-hierachy");
            m_Toolbar = root.Q("tool-bar");

            //TrackHandler
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

            // m_TrackHandleContainer.RegisterCallback<KeyDownEvent>((e) =>
            // {
            //     switch (e.keyCode)
            //     {
            //         case KeyCode.Delete:
            //         {
            //             //删除轨道
            //             Timeline.ApplyModify(() =>
            //             {
            //                 var selectableToRemove = Selections.ToList();
            //                 foreach (ISelectable selectable in selectableToRemove)
            //                 {
            //                     if (selectable is TimelineTrackHandle trackHandle)
            //                     {
            //                         Timeline.RemoveTrack(trackHandle.Track);
            //                     }
            //                 }
            //             }, "Remove");
            //             break;
            //         }
            //     }
            // });
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

            m_AddTrackButton = root.Q("add-track-button");
            m_AddTrackButton.AddManipulator(new DropdownMenuManipulator((menu) =>
            {
                // string[] acceptableTrackGroups = Timeline.GetAttribute<AcceptableTrackGroups>()?.Groups;
                // List<(Type, float)> types = new List<(Type, float)>();
                // foreach (var trackScriptPair in TimelineEditorUtility.TrackScriptMap)
                // {
                //     string trackGroup = trackScriptPair.Key.GetAttribute<TrackGroup>()?.Group ?? string.Empty;
                //     if (acceptableTrackGroups != null && !acceptableTrackGroups.Contains(trackGroup))
                //     {
                //         continue;
                //     }
                //
                //     float index = trackScriptPair.Key.GetAttribute<OrderedAttribute>()?.Index ?? 0;
                //     types.Add((trackScriptPair.Key, index));
                // }
                //
                // types = types.OrderBy(i => i.Item2).ToList();
                // foreach (var type in types.OrderBy(i => i.Item2))
                // {
                //     menu.AppendAction(type.Item1.Name, _ =>
                //     {
                //         AddTrack(type.Item1);
                //         m_TrackHandleContainer.ForceScrollViewUpdate()A;
                //     });
                // }
                foreach (var type in BBTimelineEditorUtility.BBTrackTypeDic)
                {
                    menu.AppendAction(type.Key, _ => { ApplyModify(() => { RuntimePlayable.AddTrack(type.Value); }, "Add Track"); });
                }
            }, MouseButton.LeftMouse));

            m_TimelineField = root.Q<TimelineFieldView>();
            m_TimelineField.EditorWindow = this;
            // m_TimelineField.OnPopulatedCallback += PopulateView;
            // // m_TimelineField.OnPopulatedCallback += () =>
            // // {
            // //     m_Toolbar.style.top = m_TimelineField.MarkerField.worldBound.yMin - 47;
            // // };
            // //
            // // EditorApplication.playModeStateChanged += (e) =>
            // // {
            // //     m_TargetField.SetEnabled(!Application.isPlaying);
            // //     Dispose();
            // // };
            //
            fieldScaleBar = root.Q<SliderInt>("field-scale-bar");
            fieldScaleBar.RegisterValueChangedCallback(m_TimelineField.SliderUpdate);
            Undo.undoRedoEvent += OnUndoRedoEvent;
        }

        private void OnEnable()
        {
            AssemblyReloadEvents.afterAssemblyReload += PreviewHandle;
        }

        private void OnDisable()
        {
            AssemblyReloadEvents.afterAssemblyReload -= PreviewHandle;
        }
        
        private void PreviewHandle()
        {
        }
        

        private void OnDestroy()
        {
            Undo.undoRedoEvent -= OnUndoRedoEvent;
            Dispose();
            OnDisable();
        }

        public void ApplyModify(Action action, string _name, bool rebind = true)
        {
            Undo.RegisterCompleteObjectUndo(BBTimeline, $"Timeline: {_name}");
            SerializedTimeline.Update();
            action?.Invoke();

            if (rebind) RuntimePlayable.RebindCallback?.Invoke();
            EditorUtility.SetDirty(BBTimeline);
        }

        public void Dispose()
        {
            m_TimelineField.Dispose();
        }

        public void PopulateView()
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

        public static void OpenWindow(TimelinePlayer timelinePlayer)
        {
            TimelineEditorWindow window = GetWindow<TimelineEditorWindow>();
            window.Dispose();
            window.TimelinePlayer = timelinePlayer;
            window.TimelinePlayer.Dispose();
            window.TimelinePlayer.Init();
            window.PopulateView();
        }
    }
}