using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    //从这里开始看
    public class TimelineEditorWindow: EditorWindow, ISelection
    {
        protected VisualElement m_Top;
        protected VisualElement m_LeftPanel;
        protected VisualElement m_TrackHierachy;
        protected VisualElement m_Toolbar;
        protected VisualElement m_TrackHandleContainer;
        protected VisualElement m_AddTrackButton;

        protected ObjectField m_TargetField;
        protected Button m_PlayButton;
        protected Button m_PauseButton;
        protected FloatField m_PlaySpeedField;

        protected TimelineFieldView m_TimelineField;
        public Timeline Timeline { get; private set; }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            var visualTree = Resources.Load<VisualTreeAsset>("VisualTree/TimelineEditorWindow");
            visualTree.CloneTree(root);
            root.AddToClassList("timelineEditorWindow");

            m_Top = root.Q("top");
            m_Top.SetEnabled(false);

            m_TargetField = root.Q<ObjectField>("target-field");
            m_TargetField.objectType = typeof (TimelinePlayer);
            m_TargetField.allowSceneObjects = true;
            m_TargetField.RegisterValueChangedCallback(e =>
            {
                //对象是否为持久化对象 
                //what is 持久化对象? 在 Scene 保存的 gameObject
                //不是scene中的gameObject
                if (!EditorUtility.IsPersistent(e.newValue) && e.newValue is TimelinePlayer timelinePlayer &&
                    Timeline.TimelinePlayer != timelinePlayer)
                {
                    if (Timeline.TimelinePlayer)
                    {
                        Timeline.TimelinePlayer.Dispose();
                    }

                    if (!timelinePlayer.IsValid)
                    {
                        timelinePlayer.Init();
                        timelinePlayer.AddTimeline(Timeline);
                    }
                }
                else if (e.newValue == null)
                {
                    if (Timeline.TimelinePlayer)
                    {
                        Timeline.TimelinePlayer.Dispose();
                    }
                }
                else
                {
                    m_TargetField.SetValueWithoutNotify(null);
                }
            });
            //运行时不可用
            //TODO 热重载之后改成运行时支持修改
            m_TargetField.SetEnabled(!Application.isPlaying);

            m_PlayButton = root.Q<Button>("play-button");
            m_PlayButton.clicked += () => { Timeline.TimelinePlayer.IsPlaying = true; };

            m_PauseButton = root.Q<Button>("pause-button");
            // m_PauseButton.clicked += () => { Timeline.TimelinePlayer.IsPlaying = false; };

            m_PlaySpeedField = root.Q<FloatField>("play-speed-field");
            // m_PlaySpeedField.RegisterValueChangedCallback((e) => { Timeline.TimelinePlayer.PlaySpeed = e.newValue; });

            m_LeftPanel = root.Q("left-panel");
            m_LeftPanel.SetEnabled(false);

            m_TrackHierachy = root.Q("track-hierachy");
            m_Toolbar = root.Q("tool-bar");
            m_TrackHandleContainer = root.Q("track-handle-container");
            m_TrackHandleContainer.focusable = true;
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
            //                 foreach (var selectable in selectableToRemove)
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
            // m_TrackHandleContainer.RegisterCallback<PointerDownEvent>((e) =>
            // {
            //     foreach (var timelineTrackHandle in m_TrackHandleContainer.Query<TimelineTrackHandle>().ToList())
            //     {
            //         if (timelineTrackHandle.worldBound.Contains(e.position))
            //         {
            //             timelineTrackHandle.OnPointerDown(e);
            //             e.StopImmediatePropagation();
            //             return;
            //         }
            //     }
            //
            //     if (e.button == 0)
            //     {
            //         m_TimelineField.ClearSelection();
            //         e.StopImmediatePropagation();
            //     }
            // });

            m_AddTrackButton = root.Q("add-track-button");
            m_AddTrackButton.AddManipulator(new DropdownMenuManipulator((menu) =>
            {
                string[] acceptableTrackGroups = Timeline.GetAttribute<AcceptableTrackGroups>()?.Groups;
                List<(Type, float)> types = new List<(Type, float)>();
                foreach (var trackScriptPair in TimelineEditorUtility.TrackScriptMap)
                {
                    string trackGroup = trackScriptPair.Key.GetAttribute<TrackGroup>()?.Group ?? string.Empty;
                    if (acceptableTrackGroups != null && !acceptableTrackGroups.Contains(trackGroup))
                    {
                        continue;
                    }

                    float index = trackScriptPair.Key.GetAttribute<OrderedAttribute>()?.Index ?? 0;
                    types.Add((trackScriptPair.Key, index));
                }

                types = types.OrderBy(i => i.Item2).ToList();
                foreach (var type in types.OrderBy(i=> i.Item2))
                {
                    menu.AppendAction(type.Item1.Name, _ =>
                    {
                        AddTrack(type.Item1);
                    });
                }
            }, MouseButton.LeftMouse));

            m_TimelineField = root.Q<TimelineFieldView>();
            m_TimelineField.SetEnabled(false);
            m_TimelineField.EditorWindow = this;
            m_TimelineField.OnPopulatedCallback += PopulateView;
            // m_TimelineField.OnPopulatedCallback += () =>
            // {
            //     m_Toolbar.style.top = m_TimelineField.MarkerField.worldBound.yMin - 47;
            // };
            //
            // EditorApplication.playModeStateChanged += (e) =>
            // {
            //     m_TargetField.SetEnabled(!Application.isPlaying);
            //     Dispose();
            // };

            // Undo.undoRedoEvent += OnUndoRedoEvent;
            UpdateBindState();
        }

        private void OnDestroy()
        {
            Dispose();
            Undo.undoRedoEvent += OnUndoRedoEvent;
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject is Timeline timeline && timeline != Timeline)
            {
                Dispose();
                Init(timeline);
            }
        }

        public void Init(Timeline timeline, bool initTime = true)
        {
            if (Timeline == timeline) return;

            if (initTime)
            {
                timeline.Init();
            }

            Timeline = timeline;
            Timeline.UpdateSerializedTimeline();

            Timeline.OnValueChanged += m_TimelineField.PopulateView;
            Timeline.OnEvaluated += m_TimelineField.UpdateTimeLocator;
            //Timeline.OnBindStateChanged += m_TimelineField.UpdateBindState;
            Timeline.OnBindStateChanged += UpdateBindState;

            m_Top.SetEnabled(true);
            m_LeftPanel.SetEnabled(true);
            m_TimelineField.SetEnabled(true);
            UpdateBindState();
            EditorCoroutineHelper.WaitWhile(m_TimelineField.PopulateView, () => m_TimelineField.ContentWidth == 0);
        }

        public void Dispose()
        {
            if (Timeline)
            {
                if (Timeline.TimelinePlayer && Timeline.TimelinePlayer.RunningTimelines.Count == 1 && !Application.isPlaying)
                {
                    Timeline.TimelinePlayer.Dispose();
                }

                Timeline.OnValueChanged -= m_TimelineField.PopulateView;
                Timeline.OnEvaluated -= m_TimelineField.UpdateTimeLocator;
                Timeline.OnBindStateChanged -= m_TimelineField.UpdateBindState;
                Timeline = null;
            }

            m_Top.SetEnabled(false);
            m_LeftPanel.SetEnabled(false);
            m_TimelineField.SetEnabled(false);
            UpdateBindState();
        }

        public void PopulateView()
        {
            m_TrackHandleContainer.Clear();
            m_Elements.Clear();
            m_Selections.Clear();

            if (Timeline != null)
            {
                foreach (TimelineTrackView trackView in m_TimelineField.TrackViews)
                {
                    TimelineTrackHandle trackHandle = new(trackView);
                    trackHandle.SelectionContainer = this;
                    m_TrackHandleContainer.Add(trackHandle);
                    m_Elements.Add(trackHandle);
                }
            }
        }

        private void AddTrack(Type type)
        {
            Timeline.ApplyModify(() => { Timeline.AddTrack(type); }, "Add Track");
        }

        private void OnUndoRedoEvent(in UndoRedoInfo info)
        {
            if (info.undoName.Split(':')[0] == "Timeline")
            {
                Timeline?.Init();
            }
        }

        private void UpdateBindState()
        {
            if (Timeline && Timeline.TimelinePlayer)
            {
                m_TargetField.SetValueWithoutNotify(Timeline.TimelinePlayer);
                m_PlayButton.SetEnabled(true);
                m_PauseButton.SetEnabled(true);
                m_PlaySpeedField.SetEnabled(true);
            }
            else
            {
                m_TargetField.SetValueWithoutNotify(null);
                m_PlayButton.SetEnabled(false);
                m_PauseButton.SetEnabled(false);
                m_PlaySpeedField.SetEnabled(false);
                m_PlaySpeedField.SetValueWithoutNotify(1);
            }
        }

        #region Selection

        public VisualElement ContentContainer => m_TrackHandleContainer;

        protected List<ISelectable> m_Elements = new();
        public List<ISelectable> Elements => m_Elements;

        protected List<ISelectable> m_Selections = new();
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

        [MenuItem("Tools/TimelineEditor", false, 0)]
        public static void OpenTimelineEditorWindow()
        {
            GetWindow<TimelineEditorWindow>();
        }

        [UnityEditor.Callbacks.OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId);
            var timeline = obj as Timeline;
            if (timeline == null) return false;
            GetWindow<TimelineEditorWindow>().Init(timeline);
            return true;
        }
    }
}