using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ET;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineFieldView: VisualElement, ISelection
    {
        public new class UxmlFactory: UxmlFactory<TimelineFieldView, UxmlTraits>
        {
        }

        public new class UxmlTraits: VisualElement.UxmlTraits
        {
            public UxmlTraits()
            {
                focusIndex.defaultValue = 0;
                focusable.defaultValue = true;
            }
        }

        private ScrollView TrackScrollView { get; set; }
        private VisualElement FieldContent { get; set; }
        private VisualElement TrackField { get; set; }
        private VisualElement MarkerField { get; set; }
        private VisualElement DrawFrameLineField { get; set; }
        private VisualElement TimeLocator { get; set; }
        private Label LocaterFrameLabel { get; set; }
        private ScrollView InspectorScrollView { get; set; }
        private VisualElement ClipInspector { get; set; }
        private ScrollView TrackHandleContainer { get; set; }

        #region Param

        public readonly float m_MaxFieldScale = 10;
        private readonly float m_FieldOffsetX = 6;
        private readonly float m_MarkerWidth = 30;
        public readonly float m_WheelLerpSpeed = 0.2f;
        private readonly int m_TimeTextFontSize = 14;

        #endregion

        #region Style

        private static readonly CustomStyleProperty<Color> s_FieldLineColor = new("--field-line-color");
        private Color m_FieldLineColor;
        private static readonly CustomStyleProperty<Color> s_LocatorLineColor = new("--locator-line-color");
        protected Color m_LocatorLineColor;
        private static readonly CustomStyleProperty<Font> s_MarkerTextFont = new("--marker-text-font");
        private Font m_MarkerTextFont;

        #endregion

        private float m_FieldScale = 1;
        private int m_MaxFrame = 60;
        private bool m_DrawTimeText;

        public TimelineEditorWindow EditorWindow;
        private Timeline Timeline => EditorWindow.Timeline;
        private DoubleMap<Track, TimelineTrackView> TrackViewMap { get; set; } = new();
        public List<TimelineTrackView> TrackViews { get; set; } = new();
        public Dictionary<int, float> FramePosMap { get; set; } = new();
        private DragManipulator LocatorDragManipulator { get; set; }

        public Action OnPopulatedCallback;
        public Action OnGeometryChangedCallback;

        //注意，是当前滑动窗口显示的最小帧和最大帧
        private int CurrentMinFrame => GetClosestCeilFrame(ScrollViewContentOffset);
        private int CurrentMaxFrame => GetClosestCeilFrame(ScrollViewContentWidth + ScrollViewContentOffset);
        public float OneFrameWidth => m_MarkerWidth * m_FieldScale;
        private float ScrollViewContentWidth => TrackScrollView.contentContainer.worldBound.width;
        private float ScrollViewContentOffset => TrackScrollView.scrollOffset.x;
        public float ContentWidth => FieldContent.worldBound.width;

        //当前Locator所在帧数
        private int currentTimeLocator;

        public RuntimePlayable RuntimePlayable
        {
            get
            {
                if (EditorWindow == null || EditorWindow.TimelinePlayer == null) return null;
                return EditorWindow.TimelinePlayer.RuntimeimePlayable;
            }
        }

        #region Scroll

        protected bool m_ScrollViewPan;
        private float m_ScrollViewPanDelta;
        private readonly float scrollSpeed = 3;

        #endregion

        public TimelineFieldView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineFieldView");
            visualTree.CloneTree(this);
            AddToClassList("timelineField");

            m_MarkerTextFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            TrackScrollView = this.Q<ScrollView>("track-scroll");
            TrackScrollView.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            TrackScrollView.horizontalScroller.valueChanged += _ =>
            {
                if (FieldContent.worldBound.width < ScrollViewContentWidth + ScrollViewContentOffset)
                {
                    FieldContent.style.width = ScrollViewContentWidth + ScrollViewContentOffset;
                }

                DrawTimeField();
            };
            TrackScrollView.RegisterCallback<WheelEvent>((_) =>
            {
                foreach (var child in TrackField.Children())
                {
                    if (child is not TimelineTrackView) continue;
                    ScrollView scrollView = EditorWindow.rootVisualElement.Q<ScrollView>("track-handle-container");
                    scrollView.scrollOffset = new Vector2(scrollView.scrollOffset.x, TrackScrollView.scrollOffset.y);
                    return;
                }
            });
            //水平移动
            TrackScrollView.AddManipulator(new DragManipulator((evt) => { m_ScrollViewPanDelta = evt.localPosition.x; },
                () => { },
                (v) =>
                {
                    int direction = v.x - m_ScrollViewPanDelta > 0? -1 : 1;
                    m_ScrollViewPanDelta = v.x;
                    TrackScrollView.scrollOffset += new Vector2(direction * scrollSpeed, 0);

                    UpdateTimeLocator();
                },
                (int)MouseButton.MiddleMouse));
            TrackScrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            TrackScrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;

            FieldContent = this.Q("field-content");
            FieldContent.RegisterCallback<GeometryChangedEvent>(OnTrackFieldGeometryChanged);

            TrackField = this.Q("track-field");
            TrackField.generateVisualContent += OnTrackFieldGenerateVisualContent;
            OnGeometryChangedCallback += () => { TrackField.MarkDirtyRepaint(); };

            MarkerField = this.Q("marker-field");
            MarkerField.AddToClassList("droppable");
            MarkerField.generateVisualContent += OnMarkerFieldGenerateVisualContent;
            MarkerField.RegisterCallback<PointerDownEvent>((e) =>
            {
                if (e.button == 0)
                {
                    SetTimeLocator(GetClosestFrame(e.localPosition.x + TrackScrollView.scrollOffset.x));
                    LocatorDragManipulator.DragBeginForce(e);
                }
            });
            MarkerField.SetEnabled(false);

            LocatorDragManipulator = new DragManipulator(OnTimeLocatorStartMove, OnTimeLocatorStopMove, OnTimeLocatorMove);
            TimeLocator = this.Q("time-locater");
            TimeLocator.AddManipulator(LocatorDragManipulator);
            TimeLocator.generateVisualContent += OnTimeLocatorGenerateVisualContent;
            TimeLocator.SetEnabled(false);

            DrawFrameLineField = this.Q("draw-frame-line-field");
            DrawFrameLineField.generateVisualContent += OnDrawFrameLineFieldGenerateVisualContent;

            LocaterFrameLabel = this.Q<Label>("time-locater-frame-label");

            InspectorScrollView = this.Q<ScrollView>("inspector-scroll");
            InspectorScrollView.RegisterCallback<WheelEvent>((e) => e.StopImmediatePropagation());
            ClipInspector = this.Q("clip-inspector");
            ClipInspector.focusable = true;
            ClipInspector.RegisterCallback<KeyDownEvent>((e) =>
            {
                if (!e.ctrlKey)
                {
                    e.StopImmediatePropagation();
                }
            });
            ClipInspector.RegisterCallback<PointerDownEvent>((e) => e.StopImmediatePropagation());

            RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);

            this.AddManipulator(new RectangleSelecter(() => -localBound.position));
        }

        public void PopulateView()
        {
            TrackField.Clear();
            m_Selections.Clear();
            m_Elements.Clear();
            TrackViewMap.Clear();
            TrackViews.Clear();

            ResizeTimeField();
            // PopulateInspector(null);
            UpdateBindState();

            foreach (var runtimeTrack in RuntimePlayable.RuntimeTracks)
            {
                // TrackView
                TimelineTrackView trackView = new();
                trackView.SelectionContainer = this;
                trackView.Init(runtimeTrack);

                //可以被选中
                SelectionElements.Add(trackView);
                TrackField.Add(trackView);
                TrackViews.Add(trackView);
            }

            foreach (var trackView in TrackViews)
            {
                TimelineTrackHandle trackHandle = new(trackView);
                trackHandle.SelectionContainer = this;

                EditorWindow.TrackHandleContainer.Add(trackHandle);
                SelectionElements.Add(trackHandle);
            }
            // if (Timeline)
            // {
            // Timeline.UpdateSerializedTimeline();
            //
            //     //计算最大帧长
            //     int maxFrame = 0;
            //     foreach (var track in Timeline.Tracks)
            //     {
            //         foreach (var clip in track.Clips)
            //         {
            //             if (clip.EndFrame >= maxFrame)
            //             {
            //                 maxFrame = clip.EndFrame;
            //             }
            //         }
            //     }
            //
            //     maxFrame++;
            //     m_MaxFrame = Mathf.Max(m_MaxFrame, maxFrame);
            //     ResizeTimeField();
            //     DrawTimeField();
            //
            //     foreach (var track in Timeline.Tracks)
            //     {
            //         TimelineTrackView trackView = new();
            //         trackView.SelectionContainer = this;
            //         trackView.Init(track);
            //
            //         Elements.Add(trackView);
            //         TrackField.Add(trackView);
            //         TrackViewMap.Add(track, trackView);
            //         TrackViews.Add(trackView);
            //     }
            // }
            //
            //OnPopulatedCallback?.Invoke();
        }

        private void PopulateInspector(object target)
        {
            ClipInspector.Clear();
            if (target != null)
            {
                switch (target)
                {
                    case Track track:
                    {
                        SerializedProperty serializedProperty = Timeline.SerializedTimeline.FindProperty("m_Tracks");
                        serializedProperty = serializedProperty.GetArrayElementAtIndex(Timeline.Tracks.IndexOf(track));

                        DrawProperties(serializedProperty, target);
                        break;
                    }
                    case Clip clip:
                    {
                        clip.OnInspectorRepaint = () => PopulateInspector(clip);

                        SerializedProperty serializedProperty = Timeline.SerializedTimeline.FindProperty("m_Tracks");
                        serializedProperty = serializedProperty.GetArrayElementAtIndex(Timeline.Tracks.IndexOf(clip.Track));
                        serializedProperty = serializedProperty.FindPropertyRelative("m_Clips");
                        serializedProperty = serializedProperty.GetArrayElementAtIndex(clip.Track.Clips.IndexOf(clip));

                        DrawProperties(serializedProperty, target);

                        ClipInspectorView clipViewName = clip.GetAttribute<ClipInspectorView>();
                        if (clipViewName != null)
                        {
                            foreach (var clipInspectorViewScriptPair in TimelineEditorUtility.ClipInspectorViewScriptMap)
                            {
                                if (clipInspectorViewScriptPair.Key.Name == clipViewName.Name)
                                {
                                    TimelineClipInspectorView clipInspectorView =
                                            Activator.CreateInstance(clipInspectorViewScriptPair.Key, clip) as TimelineClipInspectorView;
                                    ClipInspector.Add(clipInspectorView);
                                    return;
                                }
                            }
                        }

                        break;
                    }
                }
            }
        }

        private void DrawProperties(SerializedProperty serializedProperty, object target)
        {
            #region Base

            if (target is Clip clip)
            {
                VisualElement baseInspector = new();
                baseInspector.name = "base-inspector";
                ClipInspector.Add(baseInspector);

                IMGUIContainer baseIMGUIContainer = new(() =>
                {
                    DrawGUI("Start", clip.StartFrame);
                    DrawGUI("End", clip.EndFrame);
                    if (clip.IsMixable())
                    {
                        DrawGUI("Ease In", clip.EaseInFrame);
                        DrawGUI("Ease Out", clip.EaseOutFrame);
                        DrawGUI("ClipIn", clip.ClipInFrame);
                    }

                    DrawGUI("Duration", clip.Duration);
                });
            }

            void DrawGUI(string title, int frame)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{title}", GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                GUILayout.Label($"{(frame / (float)TimelineUtility.FrameRate):0.00}S / {frame}F", GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
            }

            #endregion

            #region Addition

            VisualElement additionalInspector = new();
            additionalInspector.name = "additional-inspector";
            ClipInspector.Add(additionalInspector);

            List<VisualElement> visualElements = new List<VisualElement>();
            Dictionary<string, (VisualElement, List<VisualElement>)> groupMap = new Dictionary<string, (VisualElement, List<VisualElement>)>();

            //blackboard中显示成员
            foreach (var fieldInfo in target.GetAllFields())
            {
                if (fieldInfo.GetCustomAttribute<ShowInInspectorAttribute>() != null)
                {
                    var showInInspectorAttribute = fieldInfo.GetCustomAttribute<ShowInInspectorAttribute>();
                    if (!fieldInfo.ShowIf(target))
                    {
                        continue;
                    }

                    if (fieldInfo.HideIf(target))
                    {
                        continue;
                    }

                    SerializedProperty sp = serializedProperty.FindPropertyRelative(fieldInfo.Name);
                    if (sp != null)
                    {
                        PropertyField propertyField = new(sp);
                        propertyField.name = showInInspectorAttribute.Index * 10 + visualElements.Count.ToString();
                        propertyField.Bind(Timeline.SerializedTimeline);

                        fieldInfo.Group(propertyField, showInInspectorAttribute.Index, ref visualElements, ref groupMap);

                        if (fieldInfo.ReadOnly(target))
                        {
                            propertyField.SetEnabled(false);
                        }

                        if (fieldInfo.GetCustomAttribute<OnValueChangedAttribute>() != null)
                        {
                            OnValueChangedAttribute onValueChanged = fieldInfo.GetCustomAttribute<OnValueChangedAttribute>();
                            EditorCoroutineHelper.Delay(() =>
                            {
                                propertyField.RegisterValueChangeCallback(_ =>
                                {
                                    foreach (var method in onValueChanged.Methods)
                                    {
                                        target.GetMethod(method)?.Invoke(target, null);
                                    }
                                });
                            }, 0.01f);
                        }
                    }
                }
            }

            //属性
            foreach (var propertyInfo in target.GetAllProperties())
            {
                if (!propertyInfo.ShowIf(target))
                {
                    continue;
                }

                if (propertyInfo.HideIf(target))
                {
                    continue;
                }

                if (propertyInfo.GetCustomAttributes<ShowTextAttribute>() is ShowTextAttribute showTextAttribute)
                {
                    IMGUIContainer container = new(() => { GUILayout.Label(propertyInfo.GetValue(target).ToString()); });
                    container.name = showTextAttribute.Index * 10 + visualElements.Count.ToString();
                    propertyInfo.Group(container, showTextAttribute.Index, ref visualElements, ref groupMap);
                }
            }

            //成员
            foreach (var methodInfo in target.GetAllMethods())
            {
                if (!methodInfo.ShowIf(target))
                {
                    continue;
                }

                if (methodInfo.HideIf(target))
                {
                    continue;
                }

                if (methodInfo.GetCustomAttribute<ShowTextAttribute>() is ShowTextAttribute showTextAttribute)
                {
                    IMGUIContainer container = new IMGUIContainer(() => { GUILayout.Label(methodInfo.Invoke(target, null).ToString()); });
                    container.name = showTextAttribute.Index * 10 + visualElements.Count.ToString();
                    methodInfo.Group(container, showTextAttribute.Index, ref visualElements, ref groupMap);
                }

                if (methodInfo.GetCustomAttributes<ButtonAttribute>() is ButtonAttribute buttonAttribute)
                {
                    Button button = new Button();
                    button.name = buttonAttribute.Index * 10 + visualElements.Count.ToString();
                    button.text = string.IsNullOrEmpty(buttonAttribute.Label)? methodInfo.Name : buttonAttribute.Label;
                    button.clicked += () => methodInfo.Invoke(target, null);
                    methodInfo.Group(button, buttonAttribute.Index, ref visualElements, ref groupMap);
                }
            }

            foreach (var visualElement in visualElements.OrderBy(i => float.Parse(i.name)))
            {
                visualElement.AddToClassList("inspectorElement");
                additionalInspector.Add(visualElement);
            }

            foreach (var groupPair in groupMap)
            {
                foreach (var groupElement in groupPair.Value.Item2.OrderBy(i => float.Parse(i.name)))
                {
                    groupElement.AddToClassList("inspectorElement");
                    groupPair.Value.Item1.Add(groupElement);
                }
            }

            #endregion
        }

        public void UpdateBindState()
        {
            if (EditorWindow == null) return;

            bool binding = (RuntimePlayable != null);
            MarkerField.SetEnabled(binding);
            TimeLocator.SetEnabled(binding);
            // PopulateInspector(Timeline);
        }

        #region Selection

        public VisualElement ContentContainer => TrackField;
        private readonly List<ISelectable> m_Elements = new();
        public List<ISelectable> SelectionElements => m_Elements;
        private readonly List<ISelectable> m_Selections = new();
        public List<ISelectable> Selections => m_Selections;

        public void AddToSelection(ISelectable selectable)
        {
            m_Selections.Add(selectable);
            selectable.Select();

            // switch (selectable)
            // {
            //     case TimelineTrackView trackView:
            //         PopulateInspector(trackView.Track);
            //         break;
            //     case TimelineClipView clipView:
            //         PopulateInspector(clipView.Clip);
            //         break;
            // }
        }

        public void RemoveFromSelection(ISelectable selectable)
        {
            m_Selections.Remove(selectable);
        }

        public void ClearSelection()
        {
            m_Selections.ForEach(i => i.UnSelect());
            Selections.Clear();

            PopulateInspector(null);
        }

        #endregion

        #region TimelineField

        public void SliderUpdate(ChangeEvent<int> evt)
        {
            m_FieldScale = evt.newValue / 100f;
            ResizeTimeField();
            DrawTimeField();
            TrackScrollView.ForceScrollViewUpdate();
            OnGeometryChangedCallback?.Invoke();
        }

        private void ResizeTimeField()
        {
            FramePosMap.Clear();

            if (FieldContent.worldBound.width < ScrollViewContentWidth + ScrollViewContentOffset)
            {
                FieldContent.style.width = ScrollViewContentWidth + ScrollViewContentOffset;
            }

            //总帧数
            int interval = Mathf.CeilToInt(Mathf.Max(FieldContent.worldBound.width, worldBound.width) / OneFrameWidth);
            if (m_MaxFrame < interval)
            {
                m_MaxFrame = interval;
            }

            for (int i = 0; i < m_MaxFrame; i++)
            {
                FramePosMap.Add(i, OneFrameWidth * i + m_FieldOffsetX);
            }

            float maxTextWidth = TextWidth(m_MaxFrame.ToString(), m_MarkerTextFont, m_TimeTextFontSize);
            m_DrawTimeText = OneFrameWidth > maxTextWidth * 1.5f;

            // resize track 
            foreach (var trackView in TrackViewMap.Values)
            {
                trackView.Refreh();
            }

            // 更新timelocator位置
            UpdateTimeLocator();
        }

        private void DrawTimeField()
        {
            TrackField.MarkDirtyRepaint();
            MarkerField.MarkDirtyRepaint();
        }

        private void OnMarkerFieldGenerateVisualContent(MeshGenerationContext mgc)
        {
            var paint2D = mgc.painter2D;
            paint2D.strokeColor = Color.white;
            paint2D.BeginPath();

            int showInterval = Mathf.CeilToInt(1 / m_FieldScale);
            int startFrame = CurrentMinFrame;
            int endFrame = CurrentMaxFrame;

            for (int j = startFrame; j <= endFrame; j++)
            {
                float pos = FramePosMap[j] - TrackScrollView.scrollOffset.x;
                //大刻度
                if (j % (showInterval * 5) == 0)
                {
                    paint2D.MoveTo(new Vector2(pos, 10));
                    paint2D.LineTo(new Vector2(pos, 25));
                    mgc.DrawText(j.ToString(), new Vector2(pos + 5, 5), m_TimeTextFontSize, Color.white);
                }
                //小刻度
                else if (j % showInterval == 0)
                {
                    paint2D.MoveTo(new Vector2(pos, 20));
                    paint2D.LineTo(new Vector2(pos, 25));

                    if (m_DrawTimeText)
                    {
                        mgc.DrawText(j.ToString(), new Vector2(pos + 5, 5), m_TimeTextFontSize, Color.white);
                    }
                }
            }

            paint2D.Stroke();
        }

        private void OnTrackFieldGenerateVisualContent(MeshGenerationContext mgc)
        {
            var paint2D = mgc.painter2D;
            paint2D.strokeColor = m_FieldLineColor;
            paint2D.BeginPath();

            int showInterval = Mathf.CeilToInt(1 / m_FieldScale);
            int startFrame = CurrentMinFrame;
            int endFrame = CurrentMaxFrame;
            for (int i = startFrame; i <= endFrame; i++)
            {
                if (i % (showInterval * 5) == 0)
                {
                    paint2D.MoveTo(new Vector2(FramePosMap[i], 0));
                    paint2D.LineTo(new Vector2(FramePosMap[i], 1000));
                }
            }

            paint2D.Stroke();
        }

        #endregion

        #region TimeLocator

        private void SetTimeLocator(int targetFrame)
        {
            currentTimeLocator = targetFrame;
            UpdateTimeLocator();
            // Timeline.TimelinePlayer.IsPlaying = false;
            // float deltaTime = targetFrame / 60f - Timeline.Time;
            // Timeline.TimelinePlayer.Evaluate(deltaTime);
        }

        public void UpdateTimeLocator()
        {
            //没有进行绑定
            if (RuntimePlayable == null) return;
            TimeLocator.style.left = FramePosMap[currentTimeLocator] - TrackScrollView.scrollOffset.x;
            TimeLocator.MarkDirtyRepaint();
            LocaterFrameLabel.text = currentTimeLocator.ToString();
            // if (Timeline != null && Timeline.Binding)
            // {
            //     TimeLocator.style.left = FramePosMap[currentTimeLocator] - TrackScrollView.scrollOffset.x;
            //     TimeLocator.MarkDirtyRepaint();
            //     // LocaterFrameLabel.text = Timeline.Frame.ToString();
            // }
            // else
            // {
            //     TimeLocator.style.left = m_FieldOffsetX;
            //     TimeLocator.MarkDirtyRepaint();
            //     LocaterFrameLabel.text = string.Empty;
            // }
        }

        private void OnTimeLocatorStartMove(PointerDownEvent evt)
        {
            LocaterFrameLabel.style.display = DisplayStyle.Flex;
        }

        private void OnTimeLocatorMove(Vector2 deltaPosition)
        {
            int targetFrame = GetClosestFrame(FramePosMap[currentTimeLocator] + deltaPosition.x);
            targetFrame = Mathf.Clamp(targetFrame, CurrentMinFrame, CurrentMaxFrame);

            currentTimeLocator = targetFrame;
            SetTimeLocator(currentTimeLocator);
        }

        private void OnTimeLocatorStopMove()
        {
            LocaterFrameLabel.style.display = DisplayStyle.None;
        }

        private void OnTimeLocatorGenerateVisualContent(MeshGenerationContext mgc)
        {
            Painter2D paint2D = mgc.painter2D;
            paint2D.strokeColor = Color.white;
            paint2D.BeginPath();
            paint2D.MoveTo(new Vector2(0, 25));
            paint2D.LineTo(new Vector2(0, 1000));
            paint2D.Stroke();
        }

        #endregion

        #region DragFrameLine

        private int[] m_DrawFrameLine = new int[0];

        public void DrawFrameLine(params int[] frames)
        {
            m_DrawFrameLine = frames;
            DrawFrameLineField.MarkDirtyRepaint();
        }

        private void OnDrawFrameLineFieldGenerateVisualContent(MeshGenerationContext mgc)
        {
            var paint2D = mgc.painter2D;
            paint2D.strokeColor = new Color(1f, 0.6f, 0f, 1f);
            paint2D.BeginPath();
            foreach (var drawFrame in m_DrawFrameLine)
            {
                int count = Mathf.CeilToInt(TrackScrollView.worldBound.height / 5);
                for (int i = 0; i < count; i += 2)
                {
                    paint2D.MoveTo(new Vector2(FramePosMap[drawFrame], i * 5));
                    paint2D.LineTo(new Vector2(FramePosMap[drawFrame], i * 5 + 5));
                }

                mgc.DrawText(drawFrame.ToString(), new Vector2(FramePosMap[drawFrame] + 5, 5), m_TimeTextFontSize, Color.white);
            }

            paint2D.Stroke();
        }

        #endregion

        #region AdjustClip

        public void ResizeClip(TimelineClipView clipView, DraglineDirection direction, float deltaPosition)
        {
            switch (direction)
            {
                case DraglineDirection.Left:
                {
                    int targetFrame = GetClosestFrame(FramePosMap[clipView.StartFrame] + deltaPosition);
                    targetFrame = Mathf.Clamp(targetFrame, CurrentMinFrame, Mathf.Min(clipView.EndFrame - 1, CurrentMaxFrame));

                    int preFrame = clipView.StartFrame;
                    clipView.BBClip.StartFrame = targetFrame;
                    bool overlap = false;

                    foreach (var clip in clipView.BBTrack.Clips)
                    {
                        if (clip == clipView.BBClip) continue;
                        if (clip.Overlap(clipView.BBClip))
                        {
                            overlap = true;
                            break;
                        }
                    }

                    clipView.BBClip.StartFrame = preFrame;
                    if (overlap) return;

                    EditorWindow.ApplyModify(() =>
                    {
                        clipView.BBClip.StartFrame = targetFrame;
                        clipView.Refresh();
                    }, "Resize Clip", false);
                    break;
                }
                case DraglineDirection.Right:
                {
                    int targetFrame = GetClosestFrame(FramePosMap[clipView.EndFrame] + deltaPosition);
                    targetFrame = Mathf.Clamp(targetFrame, Mathf.Max(clipView.StartFrame + 1, CurrentMinFrame), CurrentMaxFrame);

                    if (clipView.EndFrame == targetFrame) return;

                    //检查resize后是否和其他clip重合
                    int preFrame = clipView.EndFrame;
                    clipView.BBClip.EndFrame = targetFrame;
                    bool overlap = false;
                    foreach (var clip in clipView.BBTrack.Clips)
                    {
                        if (clip == clipView.BBClip) continue;
                        if (clip.Overlap(clipView.BBClip))
                        {
                            overlap = true;
                            break;
                        }
                    }

                    clipView.BBClip.EndFrame = preFrame;
                    if (overlap) return;

                    EditorWindow.ApplyModify(() =>
                    {
                        clipView.BBClip.EndFrame = targetFrame;
                        clipView.Refresh();
                    }, "Resize Clip", false);
                    return;
                }
            }
        }

        private TimelineClipView m_MoveLeader;
        private int m_MoveStartFrame;

        public void StartMove(TimelineClipView moveLeader)
        {
            m_MoveLeader = moveLeader;
            m_MoveStartFrame = moveLeader.StartFrame;
        }

        public void MoveClips(float deltaPosition)
        {
            int startFrame = int.MaxValue;
            int endFrame = int.MinValue;
            List<TimelineClipView> moveClips = new List<TimelineClipView>();
            foreach (ISelectable selectable in Selections)
            {
                if (selectable is TimelineClipView clipView)
                {
                    moveClips.Add(clipView);
                    if (clipView.StartFrame < startFrame)
                    {
                        startFrame = clipView.StartFrame;
                    }

                    if (clipView.EndFrame > endFrame)
                    {
                        endFrame = clipView.EndFrame;
                    }
                }
            }

            //没有需要移动的clip
            if (moveClips.Count == 0)
            {
                return;
            }

            int targetStartFrame = GetClosestFrame(FramePosMap[startFrame] + deltaPosition);
            targetStartFrame = Mathf.Clamp(targetStartFrame, CurrentMinFrame, CurrentMaxFrame);

            int deltaFrame = targetStartFrame - startFrame;

            //新的帧添加到map中
            if (deltaFrame + endFrame >= m_MaxFrame)
            {
                for (int i = m_MaxFrame; i <= deltaFrame + endFrame; i++)
                {
                    FramePosMap.Add(i, OneFrameWidth * i * m_FieldOffsetX);
                }

                m_MaxFrame = deltaFrame + endFrame + 1;
            }

            foreach (var moveClip in moveClips)
            {
                moveClip.Move(deltaFrame);
            }

            //判断产生重叠
            foreach (var moveClip in moveClips)
            {
                moveClip.BBClip.InValid = !GetMoveValid(moveClip);
            }

            //刷新视图
            UpdateMix();
            DrawFrameLine(startFrame + deltaFrame, endFrame + deltaFrame);
        }

        public void ApplyMove()
        {
            bool valid = true;
            List<TimelineClipView> moveClips = new List<TimelineClipView>();
            foreach (var selectable in Selections)
            {
                if (selectable is TimelineClipView clipView)
                {
                    moveClips.Add(clipView);
                    if (!GetMoveValid(clipView))
                    {
                        valid = false;
                    }
                }
            }

            int deltaFrame = m_MoveLeader.StartFrame - m_MoveStartFrame;
            if (deltaFrame != 0)
            {
                //OnDragStop,移动失败之后会复位
                foreach (var clipView in moveClips)
                {
                    clipView.ResetMove(deltaFrame);
                }

                if (valid)
                {
                    EditorWindow.ApplyModify(() =>
                    {
                        foreach (var clipView in moveClips)
                        {
                            clipView.Move(deltaFrame);
                        }
                    }, "Move Clip");
                }
            }

            //刷新TrackView
            UpdateMix();
            DrawFrameLine();
        }

        private bool GetMoveValid(TimelineClipView clipView)
        {
            foreach (BBClip clip in clipView.BBTrack.Clips)
            {
                if (clip == clipView.BBClip) continue;
                if (clipView.BBClip.Overlap(clip)) return false;
            }

            return true;
        }

        /// <summary>
        /// 刷新所有TrackView
        /// </summary>
        private void UpdateMix()
        {
            foreach (var trackView in TrackViews)
            {
                trackView.Refreh();
            }
        }

        #endregion

        #region Add Clip

        // public void AddClip(Track track)
        // {
        //     Clip clip = Timeline.AddClip(track, currentTimeLocator);
        //     if (clip == null) return;
        //     AdjustClip(clip);
        // }

        // public void AddClip(UnityEngine.Object referenceObject, Track track, int startFrame)
        // {
        //     AdjustClip(Timeline.AddClip(referenceObject, track, startFrame));
        // }

        /// <summary>
        /// 不能和下一个Clip重合
        /// </summary>
        // private void AdjustClip(Clip clip)
        // {
        //     Clip closetRightClip = GetClosestRightClip(clip.Track, clip.StartFrame);
        //     if (closetRightClip != null && clip.StartFrame + clip.Length > closetRightClip.StartFrame)
        //     {
        //         clip.EndFrame = closetRightClip.StartFrame;
        //         GetClipView(clip).Refresh();
        //     }
        // }

        #endregion

        #region Callback

        //获取uss中定义的变量
        private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
        {
            if (customStyle.TryGetValue(s_FieldLineColor, out Color lineColor))
            {
                m_FieldLineColor = lineColor;
            }

            if (customStyle.TryGetValue(s_MarkerTextFont, out Font textFont))
            {
                m_MarkerTextFont = textFont;
            }
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            ResizeTimeField();
            DrawTimeField();
            OnGeometryChangedCallback?.Invoke();
        }

        //进行布局计算后当元素的位置或尺寸发生变化时发送的事件。此事件无法取消，不发生涓滴，也不发生冒泡。
        private void OnTrackFieldGeometryChanged(GeometryChangedEvent evt)
        {
            float delta = evt.newRect.width - evt.oldRect.width;
            if (delta > 0)
            {
                int count = Mathf.CeilToInt(delta / OneFrameWidth);
                for (int i = 0; i < count; i++)
                {
                    FramePosMap.Add(m_MaxFrame, OneFrameWidth * m_MaxFrame + m_FieldOffsetX);
                    m_MaxFrame++;
                }
            }
        }

        #endregion

        #region Helper

        private int GetClosestFrame(float position)
        {
            int frame = 0;
            foreach (var framePosPair in FramePosMap)
            {
                if (Mathf.Abs(framePosPair.Value - position) < Mathf.Abs(FramePosMap[frame] - position))
                {
                    frame = framePosPair.Key;
                }
            }

            return frame;
        }

        public int GetCloseFrameWithin(float position, float minPosition, float maxPosition)
        {
            int frame = 0;
            foreach (var framePosPair in FramePosMap)
            {
                if (Mathf.Abs(framePosPair.Value - position) < Mathf.Abs(FramePosMap[frame] - position) && minPosition <= framePosPair.Value &&
                    framePosPair.Value <= maxPosition)
                {
                    frame = framePosPair.Key;
                }
            }

            return frame;
        }

        //向下取整
        public int GetClosestFloorFrame(float position)
        {
            int frame = 0;
            foreach (var framePosPair in FramePosMap)
            {
                if (Mathf.Abs(framePosPair.Value - position) < Mathf.Abs(FramePosMap[frame] - position) && framePosPair.Value <= position)
                {
                    frame = framePosPair.Key;
                }
            }

            return frame;
        }

        //向上取整
        private int GetClosestCeilFrame(float position)
        {
            int frame = 0;
            foreach (var framePosPair in FramePosMap)
            {
                if (Mathf.Abs(framePosPair.Value - position) < Mathf.Abs(FramePosMap[frame] - position) && position <= framePosPair.Value)
                {
                    frame = framePosPair.Key;
                }
            }

            return frame;
        }

        // public int RountToNearestInt(int value, int targetInt)
        // {
        //     int remainder = value % targetInt;
        //     if (remainder >= (float)targetInt / 2)
        //     {
        //         return value + (targetInt - remainder);
        //     }
        //     else
        //     {
        //         return value - remainder;
        //     }
        // }

        public int GetRightEdgeFrame(Track track)
        {
            int frame = 0;
            foreach (var clip in track.Clips)
            {
                if (clip.EndFrame > frame)
                {
                    frame = clip.EndFrame;
                }
            }

            return frame;
        }

        // private TimelineClipView GetClipView(Clip clip)
        // {
        //     return TrackViewMap.GetValueByKey(clip.Track).ClipViewMap.GetValueByKey(clip);
        // }

        public TimelineClipView[] GetSameTrackClipViews(TimelineClipView clipView)
        {
            return clipView.TrackView.ClipViews.ToArray();
        }

        private Clip GetClosestLeftClip(Clip targetClip)
        {
            int targetFrame = int.MinValue;
            Clip closestClip = null;
            foreach (var clip in targetClip.Track.Clips)
            {
                if (clip != targetClip && clip.StartFrame < targetClip.StartFrame && clip.StartFrame > targetFrame)
                {
                    targetFrame = clip.StartFrame;
                    closestClip = clip;
                }
            }

            return closestClip;
        }

        private Clip GetClosestRightClip(Clip targetClip)
        {
            int targetFrame = int.MaxValue;
            Clip cloestClip = null;
            foreach (var clip in targetClip.Track.Clips)
            {
                if (clip != targetClip && clip.StartFrame > targetClip.StartFrame && clip.StartFrame < targetFrame)
                {
                    targetFrame = clip.StartFrame;
                    cloestClip = clip;
                }
            }

            return cloestClip;
        }

        private Clip GetClosestRightClip(Track track, int startFrame)
        {
            List<Clip> rightClips = new();
            foreach (var clip in track.Clips)
            {
                if (clip.StartFrame > startFrame)
                {
                    rightClips.Add(clip);
                }
            }

            rightClips = rightClips.OrderBy(x => x.StartFrame).ToList();
            return rightClips.Count > 0? rightClips[0] : null;
        }

        private Clip GetOverlapClip(Clip targetClip)
        {
            foreach (var clip in targetClip.Track.Clips)
            {
                if (clip != targetClip && clip.StartFrame == targetClip.StartFrame)
                {
                    return clip;
                }
            }

            return null;
        }

        private int TextWidth(string s, Font font, int fontSize, FontStyle fontStyle = FontStyle.Normal)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }

            int w = 0;
            font.RequestCharactersInTexture(s, fontSize, fontStyle);

            foreach (char c in s)
            {
                font.GetCharacterInfo(c, out CharacterInfo cInfo, fontSize);
                w += cInfo.advance;
            }

            return w;
        }

        public int GetCurrentTimeLocator()
        {
            return currentTimeLocator;
        }

        #endregion
    }
}