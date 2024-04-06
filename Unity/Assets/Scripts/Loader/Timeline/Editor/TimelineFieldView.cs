using System;
using System.Collections.Generic;
using System.Linq;
using ET;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineFieldView: VisualElement,ISelection
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

        public ScrollView TrackScrollView { get; private set; }
        public VisualElement FieldContent { get; private set; }
        public VisualElement TrackField { get; private set; }
        public VisualElement MarkerField { get; private set; }
        public VisualElement DrawFrameLineField { get; private set; }
        public VisualElement TimeLocator { get; private set; }
        public Label LocaterFrameLabel { get; private set; }
        public ScrollView InspectorScrollView { get; private set; }
        public VisualElement ClipInspector { get; private set; }

        #region Param

        public float m_MaxFieldScale = 10;
        private readonly float m_FieldOffsetX = 6;
        private readonly float m_MarkerWidth = 50;
        public float m_WheelLerpSpeed = 0.2f;
        private readonly int m_TimeTextFontSize = 14;

        #endregion

        #region Style

        public static CustomStyleProperty<Color> s_FieldLineColor = new("--field-line-color");
        private readonly Color m_FieldLineColor;
        public static CustomStyleProperty<Color> s_LocatorLineColor = new("--locator-line-color");
        protected Color m_LocatorLineColor;
        public static CustomStyleProperty<Font> s_MarkerTextFont = new("--marker-text-font");
        private readonly Font m_MarkerTextFont;

        #endregion

        protected float m_FieldScale = 1;
        protected int m_MaxFrame = 60;
        protected bool m_DrawTimeText;
        protected bool m_ScrollViewPan;
        protected float m_ScrollViewPanDelta;

        public TimelineEditorWindow EditorWindow;
        public Timeline Timeline => EditorWindow.Timeline;
        public DoubleMap<Track, TimelineTrackView> TrackViewMap { get; private set; } = new();
        public List<TimelineTrackView> TrackViews { get; set; } = new();
        public DragManipulator LocatorDragManipulator { get; set; }

        public Action OnPopulatedCallback;
        public Action OnGeometryChangedCallback;

        public int CurrentMinFrame => GetClosestCeilFrame(ScrollViewContentOffset);
        public int CurrentMaxFrame => GetClosestCeilFrame(ScrollViewContentWidth + ScrollViewContentOffset);
        public float OneFrameWidth => m_MarkerWidth + m_FieldScale;
        public float ScrollViewContentWidth => TrackScrollView.contentContainer.worldBound.width;
        public float ScrollViewContentOffset => TrackScrollView.scrollOffset.x;
        public float ContentWidth => FieldContent.worldBound.width;

        public TimelineFieldView()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("VisualTree/TimelineFieldView");
            visualTree.CloneTree(this);
            AddToClassList("timelineField");

            m_MarkerTextFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            TrackScrollView = this.Q<ScrollView>("track-scroll");
            TrackScrollView.RegisterCallback<PointerDownEvent>((e) =>
            {
                //鼠标左键
                if (e.button == 2)
                {
                    m_ScrollViewPan = true;
                    m_ScrollViewPanDelta = e.localPosition.x;
                    TrackField.AddToClassList("pan");
                }
            });
            TrackScrollView.RegisterCallback<PointerDownEvent>((e) =>
            {
                if (m_ScrollViewPan)
                {
                    TrackScrollView.scrollOffset = new Vector2(TrackScrollView.scrollOffset.x + m_ScrollViewPanDelta - e.localPosition.x,
                        TrackScrollView.scrollOffset.y);
                    m_ScrollViewPanDelta = e.localPosition.x;
                }
            });
            TrackScrollView.RegisterCallback<PointerOutEvent>((e) =>
            {
                m_ScrollViewPan = false;
                TrackField.RemoveFromClassList("pan");
            });
            TrackScrollView.RegisterCallback<PointerUpEvent>((e) =>
            {
                m_ScrollViewPan = false;
                TrackField.RemoveFromClassList("pan");
            });
            TrackScrollView.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            TrackScrollView.horizontalScroller.valueChanged += (e) =>
            {
                if (FieldContent.worldBound.width < ScrollViewContentWidth + ScrollViewContentOffset)
                {
                    FieldContent.style.width = ScrollViewContentWidth + ScrollViewContentOffset;
                }

                DrawTimeField();
            };

            FieldContent = this.Q("field-content");
            FieldContent.RegisterCallback<GeometryChangedEvent>(OnTrackFieldGeometryChanged);

            TrackField = this.Q("track-field");
            TrackField.generateVisualContent += OnTrackFieldGenerateVisualContent;

            MarkerField = this.Q("marker-field");
            MarkerField.AddToClassList("droppable");
            MarkerField.generateVisualContent += OnMarkerFieldGenerateVisualContent;
            MarkerField.RegisterCallback<PointerDownEvent>((e) =>
            {
                //鼠标左键
                if (e.button == 0)
                {
                    SettimeLocator(GetCloseFrame(e.localPosition.x));
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
            ClipInspector.RegisterCallback<KeyDownEvent>((e) =>
            {
                if (!e.ctrlKey)
                {
                    e.StopImmediatePropagation();
                }
            });
            ClipInspector.RegisterCallback<PointerDownEvent>((e) => e.StopImmediatePropagation());

            RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
            RegisterCallback<WheelEvent>(OnWheelEvent);
            RegisterCallback<KeyDownEvent>((e) =>
            {
                switch (e.keyCode)
                {
                    case KeyCode.Delete:
                    {
                        Timeline.ApplyModify(() =>
                        {
                            var selectableToRemove = Selections.ToList();
                            foreach (var selectable in selectableToRemove)
                            {
                                if (selectable is TimelineTrackView trackView)
                                {
                                    Timeline.RemoveTrack(trackView.Track);
                                }

                                if (selectable is TimelineClipView clipView)
                                {
                                    Timeline.RemoveClip(clipView.Clip);
                                }
                            }
                        }, "Remove");
                        break;
                    }
                    case KeyCode.F:
                    {
                        int startFrame = int.MaxValue;
                        int endFrame = int.MinValue;
                        foreach (var track in Timeline.Tracks)
                        {
                            foreach (var clip in track.Clips)
                            {
                                if (clip.StartFrame < startFrame)
                                {
                                    startFrame = clip.StartFrame;
                                }

                                if (clip.EndFrame >= endFrame)
                                {
                                    endFrame = clip.EndFrame;
                                }
                            }

                            int middleFrame = (startFrame + endFrame) / 2;
                            TrackScrollView.scrollOffset = new Vector2(middleFrame * OneFrameWidth, TrackScrollView.scrollOffset.y);
                        }

                        break;
                    }
                }
            });

            this.AddManipulator(new RectangleSelecter(() => -localBound.position));
        }

        public void PopulateView()
        {
            TrackField.Clear();
            m_Selections.Clear();
            m_Elements.Clear();
            TrackViewMap.Clear();
            TrackViews.Clear();
            PopulateInspector(null);
            UpdateBindState();

            if (Timeline)
            {
                Timeline.UpdateSerializedTimeline();

                int maxFrame = 0;
                foreach (var track in Timeline.Tracks)
                {
                    foreach (var clip in track.Clips)
                    {
                        if (clip.EndFrame >= maxFrame)
                        {
                            maxFrame = clip.EndFrame;
                        }
                    }
                }

                maxFrame++;

                m_MaxFrame = Mathf.Max(m_MaxFrame, maxFrame);
                m_FieldScale = Timeline.Scale;
                
                ResizeTimeField();
                DrawTimeField();

                foreach (var track in Timeline.Tracks)
                {
                    TimelineTrackView trackView = new();
                    trackView.SelectionContainer = this;
                    trackView.Init(track);
                    
                    Elements.Add(trackView);
                    TrackField.Add(trackView);
                    TrackViewMap.Add(track,trackView);
                    TrackViews.Add(trackView);
                }
            }
            
            OnPopulatedCallback?.Invoke();
        }

        public void PopulateInspector(object target)
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
                        
                        DrawProperties(serializedProperty,target);
                        break;
                    }
                    case Clip clip:
                    {
                        clip.OnInspectorRepaint = () => PopulateInspector(clip);

                        SerializedProperty serializedProperty = Timeline.SerializedTimeline.FindProperty("m_Tracks");
                        serializedProperty = serializedProperty.GetArrayElementAtIndex(Timeline.Tracks.IndexOf(clip.Track));
                        serializedProperty = serializedProperty.FindPropertyRelative("m_Clips");
                        serializedProperty = serializedProperty.GetArrayElementAtIndex(clip.Track.Clips.IndexOf(clip));
                        
                        DrawProperties(serializedProperty,target);
                        
                        break;
                    }
                }
            }
        }

        public void DrawProperties(SerializedProperty serializedProperty, object target)
        {
            
        }

        public void UpdateBindState()
        {
            
        }

        public void ForceScrollViewUpdate(ScrollView view)
        {
            
        }
        
        public Dictionary<int, float> FramePosMap { get; set; }

        #region Selection
        public VisualElement ContentContainer => TrackField;
        protected List<ISelectable> m_Elements = new();
        public List<ISelectable> Elements => m_Elements;
        protected List<ISelectable> m_Selections = new();
        public List<ISelectable> Selections => m_Selections;

        public void AddToSelection(ISelectable selectable)
        {
            m_Selections.Add(selectable);
            selectable.Select();

            if (selectable is TimelineTrackView trackView)
            {
            }
        }

        public void RemoveFromSelection(ISelectable selectable)
        {
            
        }

        public void ClearSelection()
        {
            
        }
        
        #endregion
        
        #region TimelineField

        public void ResizeTimeField()
        {
            FramePosMap.Clear();

            if (FieldContent.worldBound.width < ScrollViewContentWidth + ScrollViewContentOffset)
            {
                FieldContent.style.width = ScrollViewContentWidth + ScrollViewContentOffset;
            }

            // 算占用帧数?
            int interval = Mathf.CeilToInt(Mathf.Max(FieldContent.worldBound.width, worldBound.width) / OneFrameWidth);
            if (m_MaxFrame < interval)
            {
                m_MaxFrame = interval;
            }

            for (int i = 0; i < m_MaxFrame; i++)
            {
                FramePosMap.Add(i, OneFrameWidth * i * m_FieldOffsetX);
            }

            float maxTextWidth = TextWidth(m_MaxFrame.ToString(), m_MarkerTextFont, m_TimeTextFontSize);
            m_DrawTimeText = OneFrameWidth > maxTextWidth * 1.5f;
        }

        public void DrawTimeField()
        {
            //重绘
            TrackField.MarkDirtyRepaint();
            MarkerField.MarkDirtyRepaint();
        }

        private void OnMarkerFieldGenerateVisualContent(MeshGenerationContext mgc)
        {
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
                    paint2D.LineTo(new Vector2(FramePosMap[i], TrackScrollView.worldBound.height));
                }
            }

            paint2D.Stroke();
        }

        #endregion

        #region TimeLocator

        public void SettimeLocator(int targetFrame)
        {
        }

        public void UpdateTimeLocator()
        {
        }

        private void OnTimeLocatorStartMove(PointerDownEvent evt)
        {
        }

        private void OnTimeLocatorMove(Vector2 deltaPosition)
        {
        }

        private void OnTimeLocatorStopMove()
        {
        }

        private void OnTimeLocatorGenerateVisualContent(MeshGenerationContext mgc)
        {
        }

        #endregion

        #region DragFrameLine

        private int[] m_DrawFrameLine = new int[0];

        public void DrawFrameLine(params int[] frames)
        {
            this.m_DrawFrameLine = frames;
            this.DrawFrameLineField.MarkDirtyRepaint();
        }

        private void OnDrawFrameLineFieldGenerateVisualContent(MeshGenerationContext mgc)
        {
        }

        #endregion

        #region AdjustClip

        public void ResizeClip(TimelineClipView clipView, int border, float deltaPosition)
        {
        }

        public void AdjustSelfEase(TimelineClipView clipView, int border, float deltaPosition)
        {
        }

        private TimelineClipView m_MoveLeader;
        private int m_MoveStartFrame;

        public void StartMove(TimelineClipView moveLeader)
        {
        }

        public void MoveClips(float deltaPosition)
        {
        }

        public void ApplyMove()
        {
        }

        public bool GetMoveValid(TimelineClipView clipView)
        {
            return false;
        }

        #endregion

        #region Add Clip

        public void AddClip(Track track, int startFrame)
        {
        }

        public void AddClip(UnityEngine.Object referenceObject, Track track, int startFrame)
        {
        }

        private void AdjustClip(Clip clip)
        {
        }

        #endregion

        #region Callback

        private void OnCustomStyleResolved(CustomStyleResolvedEvent evt)
        {
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
        }

        private void OnTrackFieldGeometryChanged(GeometryChangedEvent evt)
        {
        }

        private void OnWheelEvent(WheelEvent wheelEvent)
        {
        }

        #endregion

        #region Helper

        public int GetCloseFrame(float position)
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
        public int GetClosestCeilFrame(float position)
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

        public TimelineClipView GetClipView(Clip clip)
        {
            return TrackViewMap.GetValueByKey(clip.Track).ClipViewMap.GetValueByKey(clip);
        }

        public TimelineClipView[] GetSameTrackClipViews(TimelineClipView clipView)
        {
            return clipView.TrackView.ClipViews.ToArray();
        }

        public Clip GetClosestLeftClip(Clip targetClip)
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

        public Clip GetClosestRightClip(Clip targetClip)
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

        public Clip GetClosestRightClip(Track track, int startFrame)
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

        public Clip GetOverlapClip(Clip targetClip)
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

        public int TextWidth(string s, Font font, int fontSize, FontStyle fontStyle = FontStyle.Normal)
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

        #endregion
    }
}