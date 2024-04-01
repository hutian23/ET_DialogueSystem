using System;
using System.Collections.Generic;
using ET;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineFieldView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TimelineFieldView,UxmlTraits> { }

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
        protected bool m_ScrollViewPanDelta;

        public TimelineEditorWindow EditorWindow;
        public Timeline Timeline => EditorWindow.Timeline;
        public DoubleMap<Track, TimelineTrackView> TrackViewMap { get; private set; } = new();
        public List<TimelineTrackView> TrackViews { get; set; } = new();
        public DragManipulator LocatorDragManipulator { get; set; }
        
        // public MultiMap<Track>
        // public Timeline Timeline => 


        public Action OnPopulatedCallback;
        public Action OnGeometryChangedCallback;
        // public int CurrentMinFrame => GetClos
        
        public Dictionary<int,float> FramePosMap { get; set; }

        public VisualElement ContentContainer { get; }
        public List<ISelectable> Elements { get; }
        public List<ISelectable> Selections { get; }
        public List<ISelectable> selection { get; }

        #region Helper

        public int GetCloseFrame(float position)
        {
            int frame = 0;
            return frame;
        }
        

        #endregion
    }
}