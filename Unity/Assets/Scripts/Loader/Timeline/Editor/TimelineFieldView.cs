using System;
using System.Collections.Generic;
using System.Linq;
using ET;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineFieldView: VisualElement
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
        public Label LocaterFrameLabel { get; private set; }
        public ScrollView InspectorScrollView { get; private set; }
        public VisualElement ClipInspector { get; private set; }

        #region Param

        public float m_MaxFieldScale = 10;
        public float m_FieldOffsetX = 6;
        public float m_MarkerWidth = 50;
        public float m_WheelLerpSpeed = 0.2f;
        public int m_TimeTextFontSize = 14;

        #endregion

        #region Style

        public static CustomStyleProperty<Color> s_FieldLineColor = new("--field-line-color");
        protected Color m_FieldLineColor;
        public static CustomStyleProperty<Color> s_LocatorLineColor = new("--locator-line-color");
        protected Color m_LocatorLineColor;
        public static CustomStyleProperty<Font> s_MarkerTextFont = new("--marker-text-font");
        protected Font m_MarkerTextFont;

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

        public int CurrentMinFrame;
        public int CurrentMaxFrame;
        public float OneFrameWidth;
        public float ScrollViewContentWidth;
        public float ScrollViewContentOffset;
        public float ContentWidth;

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
            TrackScrollView.horizontalScroller.valueChanged += (e) =>
            {
                if (FieldContent.worldBound.width < ScrollViewContentWidth + ScrollViewContentOffset)
                {
                    FieldContent.style.width = ScrollViewContentWidth + ScrollViewContentOffset;
                }
            };

            MarkerField.generateVisualContent += OnMarkerFieldGenerateVisualContent;
        }

        public Dictionary<int, float> FramePosMap { get; set; }

        public VisualElement ContentContainer { get; }
        public List<ISelectable> Elements { get; }
        public List<ISelectable> Selections { get; }
        public List<ISelectable> selection { get; }

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

            float maxTextWidth = TextWidth(m_MaxFrame.ToString(),m_MarkerTextFont, m_TimeTextFontSize);
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
            font.RequestCharactersInTexture(s,fontSize,fontStyle);

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