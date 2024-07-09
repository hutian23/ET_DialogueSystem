using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
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
        public VisualElement ClipInspector { get; set; }
        // private VisualElement MarkerViewField { get; set; }

        #region Param

        public readonly float m_MaxFieldScale = 10;
        private readonly float m_FieldOffsetX = 5;
        private readonly float m_MarkerWidth = 30;
        public readonly float m_WheelLerpSpeed = 0.2f;
        private readonly int m_TimeTextFontSize = 12;

        #endregion

        #region Style

        private static readonly CustomStyleProperty<Color> s_FieldLineColor = new("--field-line-color");
        private Color m_FieldLineColor;
        protected Color m_LocatorLineColor;
        private static readonly CustomStyleProperty<Font> s_MarkerTextFont = new("--marker-text-font");
        private Font m_MarkerTextFont;

        #endregion

        private float m_FieldScale = 1;
        private int m_MaxFrame = 60;
        private bool m_DrawTimeText;

        public TimelineEditorWindow EditorWindow;
        public List<TimelineTrackView> TrackViews { get; set; } = new();
        private List<TimelineMarkerView> MarkerViews { get; set; } = new();
        public Dictionary<int, float> FramePosMap { get; set; } = new();
        private DragManipulator LocatorDragManipulator { get; set; }

        public Action OnPopulatedCallback;
        private readonly Action OnGeometryChangedCallback;

        //注意，是当前滑动窗口显示的最小帧和最大帧
        public int CurrentMinFrame => GetClosestCeilFrame(ScrollViewContentOffset);
        public int CurrentMaxFrame => GetClosestCeilFrame(ScrollViewContentWidth + ScrollViewContentOffset);
        private float OneFrameWidth => m_MarkerWidth * m_FieldScale;
        private float ScrollViewContentWidth => TrackScrollView.contentContainer.worldBound.width;
        public float ScrollViewContentOffset => TrackScrollView.scrollOffset.x;

        public float ContentWidth => FieldContent.worldBound.width;

        //当前Locator所在帧数
        private int currentTimeLocator;

        private RuntimePlayable RuntimePlayable
        {
            get
            {
                if (EditorWindow == null || EditorWindow.TimelinePlayer == null) return null;
                return EditorWindow.TimelinePlayer.RuntimeimePlayable;
            }
        }

        private IShowInspector currentInspector;

        protected bool m_ScrollViewPan;
        private float m_ScrollViewPanDelta;
        private readonly float scrollSpeed = 3;

        private EditorCoroutine PlayCor;

        public TimelineFieldView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineFieldView");
            visualTree.CloneTree(this);
            AddToClassList("timelineField");

            m_MarkerTextFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            TrackScrollView = this.Q<ScrollView>("track-scroll");
            TrackScrollView.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            TrackScrollView.horizontalScroller.valueChanged += _ => { DrawTimeField(); };
            TrackScrollView.RegisterCallback<WheelEvent>((_) =>
            {
                foreach (VisualElement child in TrackField.Children())
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

                    // Refill FieldContent
                    Vector2 scrollOffset = TrackScrollView.scrollOffset + new Vector2(direction * scrollSpeed, 0);
                    if (FieldContent.worldBound.width < scrollOffset.x + ScrollViewContentWidth)
                    {
                        FieldContent.style.width = scrollOffset.x + ScrollViewContentWidth;
                    }

                    TrackScrollView.scrollOffset = scrollOffset;

                    UpdateTimeLocator();
                    UpdateMix();
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
                    StopPlayTimelineCor();
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

            //Marker
            // MarkerViewField = this.Q<VisualElement>("marker-view");
            // MarkerViewField.RegisterCallback<PointerDownEvent>(MarkerViewPointerDown);

            RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
            this.AddManipulator(new RectangleSelecter(() => -localBound.position));
        }

        public void Dispose()
        {
            //Destroy inspectorData
            currentInspector?.InspectorDestroy();
            currentInspector = null;
            ClipInspector.Clear();

            //Stop play Cor
            if (PlayCor != null)
            {
                EditorCoroutineUtility.StopCoroutine(PlayCor);
            }

            TrackField.Clear();
            m_Selections.Clear();
            m_Elements.Clear();
            TrackViews.Clear();
            MarkerViews.Clear();
            // MarkerViewField.Clear();
        }

        public void PopulateView()
        {
            Dispose();

            //get maxframe
            int maxFrame = m_MaxFrame;
            // foreach (MarkerInfo mark in RuntimePlayable.Timeline.Marks)
            // {
            //     if (mark.frame >= maxFrame)
            //     {
            //         maxFrame = mark.frame + 1;
            //     }
            // }
            foreach (RuntimeTrack runtimeTrack in RuntimePlayable.RuntimeTracks)
            {
                if (maxFrame <= runtimeTrack.Track.GetMaxFrame())
                {
                    maxFrame = runtimeTrack.Track.GetMaxFrame() + 1;
                }
            }

            m_MaxFrame = maxFrame;

            ResizeTimeField();
            UpdateBindState();

            foreach (var runtimeTrack in RuntimePlayable.RuntimeTracks)
            {
                // TrackView
                TimelineTrackView trackView = Activator.CreateInstance(runtimeTrack.Track.TrackViewType) as TimelineTrackView;
                trackView.SelectionContainer = this;
                trackView.Init(runtimeTrack);

                //可以被选中
                SelectionElements.Add(trackView);
                TrackField.Add(trackView);
                TrackViews.Add(trackView);
            }

            foreach (TimelineTrackView trackView in TrackViews)
            {
                TimelineTrackHandle trackHandle = new(trackView);
                trackHandle.SelectionContainer = this;

                EditorWindow.TrackHandleContainer.Add(trackHandle);
                SelectionElements.Add(trackHandle);
            }

            foreach (MarkerInfo marker in RuntimePlayable.Timeline.Marks)
            {
                TimelineMarkerView markerView = new();
                markerView.SelectionContainer = this;
                markerView.Init(marker);

                // MarkerViewField.Add(markerView);
                SelectionElements.Add(markerView);
                MarkerViews.Add(markerView);
            }
            
            UpdateTimeLocator();
        }

        private void UpdateBindState()
        {
            if (EditorWindow == null) return;

            bool binding = (RuntimePlayable != null);
            MarkerField.SetEnabled(binding);
            TimeLocator.SetEnabled(binding);
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

            //can show in inspector
            if (selectable is not IShowInspector _showInspector) return;
            currentInspector?.InspectorDestroy();
            currentInspector = _showInspector;
            currentInspector?.InspectorAwake();
        }

        public void RemoveFromSelection(ISelectable selectable)
        {
            m_Selections.Remove(selectable);
            if (selectable.Equals(currentInspector))
            {
                currentInspector.InspectorDestroy();
                currentInspector = null;
            }
        }

        public void ClearSelection()
        {
            m_Selections.ForEach(i =>
            {
                i.UnSelect();
                if (i.Equals(currentInspector))
                {
                    currentInspector.InspectorDestroy();
                    currentInspector = null;
                }
            });
            Selections.Clear();
        }

        #endregion

        #region TimelineField

        public void SliderUpdate(ChangeEvent<int> evt)
        {
            m_FieldScale = evt.newValue / 100f;
            ResizeTimeField();
            UpdateMix();
            DrawTimeField();
            TrackScrollView.ForceScrollViewUpdate();
            OnGeometryChangedCallback?.Invoke();
        }

        public void ResizeTimeField(int maxFrame)
        {
            if (maxFrame < m_MaxFrame) return;
            m_MaxFrame = maxFrame;
            ResizeTimeField();
        }

        private void ResizeTimeField()
        {
            FramePosMap.Clear();

            for (int i = 0; i < m_MaxFrame; i++)
            {
                FramePosMap.Add(i, OneFrameWidth * i + m_FieldOffsetX);
            }

            float maxTextWidth = TextWidth(m_MaxFrame.ToString(), m_MarkerTextFont, m_TimeTextFontSize);
            m_DrawTimeText = OneFrameWidth > maxTextWidth * 1.5f;

            //Repaint marker 
            DrawTimeField();

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

        public void CurrentFrameFieldUpdate(int currentFrame)
        {
            bool InRange = currentFrame < CurrentMaxFrame && currentFrame > CurrentMinFrame;
            float preMaxFramePos = FramePosMap[CurrentMaxFrame];

            currentTimeLocator = currentFrame;
            m_MaxFrame = Mathf.Max(currentFrame + 1, m_MaxFrame);
            ResizeTimeField();

            if (!InRange)
            {
                float currentLocatorPos = FramePosMap[currentTimeLocator];
                TrackScrollView.scrollOffset += new Vector2(currentLocatorPos - preMaxFramePos + 50, 0);
            }

            ResizeTimeField();
            DrawTimeField();
        }

        private void SetTimeLocator(int targetFrame)
        {
            currentTimeLocator = targetFrame;
            UpdateTimeLocator();
        }

        private void UpdateTimeLocator()
        {
            //没有进行绑定
            if (RuntimePlayable == null) return;
            TimeLocator.style.left = FramePosMap[currentTimeLocator] - TrackScrollView.scrollOffset.x;
            TimeLocator.MarkDirtyRepaint();
            LocaterFrameLabel.text = currentTimeLocator.ToString();

            //更新Inspector
            currentInspector?.InsepctorUpdate();
            //更新frameField
            // EditorWindow.m_currentFrameField.SetValueWithoutNotify(currentTimeLocator);
            // string Marker = "_ _ _";
            // foreach (MarkerInfo marker in EditorWindow.BBTimeline.Marks)
            // {
            //     if (marker.frame != currentTimeLocator) continue;
            //     Marker = marker.markerName;
            // }
            //
            // EditorWindow.m_currentMarkerField.SetValueWithoutNotify(Marker);

            //更新playableGraph
            RuntimePlayable.Evaluate(currentTimeLocator);
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

        #region Marker

        // private void MarkerViewPointerDown(PointerDownEvent evt)
        // {
        //     int targetFrame = GetClosestFrame(evt.localPosition.x + ScrollViewContentOffset);
        //     foreach (TimelineMarkerView marker in MarkerViews)
        //     {
        //         //不知道为什么，这里addToSelection不会改变样式
        //         if (!marker.InMiddle(targetFrame))
        //         {
        //             continue;
        //         }
        //
        //         marker.OnPointerDown(evt);
        //         return;
        //     }
        // }

        private int m_StartMoveMarkerFrame;

        public void MarkerStartMove(TimelineMarkerView markerView)
        {
            m_StartMoveMarkerFrame = markerView.info.frame;
        }

        public void MoveMarkers(float deltaPosition)
        {
            int startFrame = int.MaxValue;
            List<TimelineMarkerView> moveMarkers = new List<TimelineMarkerView>();
            foreach (ISelectable selectable in Selections)
            {
                if (selectable is TimelineMarkerView markerView)
                {
                    moveMarkers.Add(markerView);
                    if (markerView.info.frame < startFrame)
                    {
                        startFrame = markerView.info.frame;
                    }
                }
            }

            if (moveMarkers.Count == 0)
            {
                return;
            }

            int targetStartFrame = GetClosestFrame(FramePosMap[startFrame] + deltaPosition);
            targetStartFrame = Mathf.Clamp(targetStartFrame, CurrentMinFrame, CurrentMaxFrame);

            int deltaFrame = targetStartFrame - startFrame;

            foreach (TimelineMarkerView markerView in moveMarkers)
            {
                markerView.Move(deltaFrame);
            }

            //Resize frameMap
            int maxFrame = int.MinValue;
            foreach (var marker in moveMarkers)
            {
                if (marker.info.frame >= maxFrame)
                {
                    maxFrame = marker.info.frame;
                }
            }

            if (maxFrame >= m_MaxFrame)
            {
                for (int i = m_MaxFrame; i <= maxFrame; i++)
                {
                    FramePosMap.Add(i, OneFrameWidth * i + m_FieldOffsetX);
                }

                m_MaxFrame = maxFrame + 1;
            }

            //判断当前移动的marker是否发生重叠
            foreach (TimelineMarkerView marker in moveMarkers)
            {
                marker.InValid = GetMarkerMoveValid(marker);
            }

            UpdateMix();
            DrawFrameLine(startFrame);
        }

        public void ApplyMarkerMove()
        {
            int startFrame = int.MaxValue;
            bool InValid = true;

            List<TimelineMarkerView> moveMarkers = new();
            foreach (var selection in Selections)
            {
                if (selection is not TimelineMarkerView markerView) continue;

                // override with other marker
                if (!markerView.InValid)
                {
                    InValid = false;
                }

                if (markerView.info.frame <= startFrame)
                {
                    startFrame = markerView.info.frame;
                }

                moveMarkers.Add(markerView);
            }

            int deltaFrame = startFrame - m_StartMoveMarkerFrame;

            if (deltaFrame != 0)
            {
                //Reset Position
                foreach (var markerView in moveMarkers)
                {
                    markerView.ResetMove(deltaFrame);
                }

                if (InValid)
                {
                    EditorWindow.ApplyModify(() =>
                    {
                        foreach (var markerView in moveMarkers)
                        {
                            markerView.Move(deltaFrame);
                        }
                    }, "Move Marker");
                }
            }

            DrawFrameLine();
            UpdateMix();
        }

        private bool GetMarkerMoveValid(TimelineMarkerView markerView)
        {
            foreach (var view in MarkerViews)
            {
                if (view == markerView)
                {
                    continue;
                }

                if (view.info.frame == markerView.info.frame)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region DragFrameLine

        private int[] m_DrawFrameLine = Array.Empty<int>();

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
                if (clip == clipView.BBClip)
                {
                    continue;
                }

                if (clipView.BBClip.Overlap(clip))
                {
                    return false;
                }
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
                trackView.Refresh();
            }

            //Update marker pos
            foreach (var markerView in MarkerViews)
            {
                markerView.Refresh();
            }
        }

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

        public int GetClosestFrame(float position)
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

        #region Play

        public void StopPlayTimelineCor()
        {
            if (PlayCor != null) EditorCoroutineUtility.StopCoroutine(PlayCor);
        }

        public void PlayTimelineCor()
        {
            if (PlayCor != null)
            {
                EditorCoroutineUtility.StopCoroutine(PlayCor);
            }

            PlayCor = EditorCoroutineUtility.StartCoroutine(PlayTimelineCoroutine(), this);
        }

        public void LoopPlayTimelineCor()
        {
            if (PlayCor != null)
            {
                EditorCoroutineUtility.StopCoroutine(PlayCor);
            }

            PlayCor = EditorCoroutineUtility.StartCoroutine(LoopPlayCoroutine(), this);
        }

        private int GetMaxFrame()
        {
            int maxFrame = 0;
            foreach (BBTrack track in RuntimePlayable.Timeline.Tracks)
            {
                if (maxFrame <= track.GetMaxFrame())
                {
                    maxFrame = track.GetMaxFrame();
                }
            }

            return maxFrame;
        }

        private IEnumerator PlayTimelineCoroutine()
        {
            float counter = 0f;
            SetTimeLocator(0);
            while (currentTimeLocator < GetMaxFrame())
            {
                SetTimeLocator((int)(counter * TimelineUtility.FrameRate));
                //AnimationClip 当前preview模式下会阻塞主线程?(preview中timeline更新速率变慢)

                float timer = Time.realtimeSinceStartup;
                yield return null;
                counter += Time.realtimeSinceStartup - timer;
            }
        }

        private IEnumerator LoopPlayCoroutine()
        {
            while (true)
            {
                yield return PlayTimelineCoroutine();
                yield return null;
            }
        }

        #endregion
    }
}