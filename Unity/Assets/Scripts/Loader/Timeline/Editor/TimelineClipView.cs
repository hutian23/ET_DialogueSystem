using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineClipView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TimelineClipView,UxmlTraits> {}

        protected string m_DefaultVisualTreeGuid = "";
        protected virtual string VisualTreeGuid => m_DefaultVisualTreeGuid;
        
        public bool Selected { get; private set; }
        public bool Hoverd { get; private set; }
        public ISelection SelectionContainer { get; set; }
        public ClipCapabilities Capabilities;

        public TimelineFieldView FieldView;
        public TimelineEditorWindow EditorWindow;
        public Timeline Timeline;
        public Dictionary<int, float> FramePosMap => FieldView.FramePosMap;
        public Clip Clip { get; private set; }
        public TimelineTrackView TrackView { get; private set; }

        public int StartFrame => Clip.StartFrame;
        public int EndFrame => Clip.EndFrame;
        public int OtherEaseInFrame => Clip.OtherEaseInFrame;
        public int OtherEaseOutFrame => Clip.OtherEaseOutFrame;
        public int SelfEaseInFrame => Clip.SelfEaseInFrame;
        public int SelfEaseOutFrame => Clip.SelfEaseOutFrame;
        public int EaseInFrame => Clip.EaseInFrame;
        public int EaseOutFrame => Clip.EaseOutFrame;
        public int ClipInFrame => Clip.ClipInFrame;
        public int WidthFrame => EndFrame - StartFrame;

        protected DragLineManipulator m_LeftResizeDragLine;
        protected DragLineManipulator m_SelfEaseInDragLine;
        protected DragLineManipulator m_RightResizeDragLine;
        protected DragLineManipulator m_SelfEaseOutDragLine;
        protected DragManipulator m_MoveDrag;
        protected DropdownMenuHandler m_MenuHandle;

        protected VisualElement m_Content;
        protected VisualElement m_LeftMixer;
        protected VisualElement m_RightMixer;
        protected VisualElement m_Title;
        protected Label m_ClipName;
        protected VisualElement m_LeftClipIn;
        protected VisualElement m_RightClipIn;
        protected VisualElement m_BottomLine;
        protected VisualElement m_DrawBox;

        public bool SelfEaseIn
        {
            get
            {
                return m_SelfEaseInDragLine?.Enable ?? false;
            }
            set
            {
                if (m_SelfEaseInDragLine != null)
                {
                    m_SelfEaseInDragLine.Enable = value;
                    m_LeftResizeDragLine.Enable = !value;
                }
            }
        }

        public bool SelfEaseOut
        {
            get
            {
                return m_SelfEaseOutDragLine?.Enable ?? false;
            }
            set
            {
                if (m_SelfEaseOutDragLine != null)
                {
                    m_SelfEaseOutDragLine.Enable = value;
                    m_RightResizeDragLine.Enable = !value;
                }
            }
        }

        public TimelineClipView()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AssetDatabase.GUIDToAssetPath(m_DefaultVisualTreeGuid));
            visualTree.CloneTree(this);
            AddToClassList("timelineClip");

            m_Content = this.Q("content");
            m_LeftMixer = this.Q("left-mixer");
            m_RightMixer = this.Q("right-mixer");

            m_Title = this.Q("title");
            m_ClipName = this.Q<Label>("clip-name");
            m_LeftClipIn = this.Q("left-clip-in");
            m_RightClipIn = this.Q("right-clip-in");
            m_BottomLine = this.Q("bottom-line");
            m_DrawBox = this.Q("draw-box");

            m_MoveDrag = new DragManipulator(OnStartDrag, OnStopDrag, OnDragMove);
            m_MoveDrag.enabled = false;
            this.AddManipulator(m_MoveDrag);
            
            // m_MenuHandle = new DropdownMenuHandler()
            // m_DrawBox.generateVisualContent += O
        }

        public void Init(Clip clip, TimelineTrackView trackView)
        {
            Clip = clip;
            Clip.OnNameChanged = () => m_ClipName.text = clip.Name;
            m_ClipName.text = clip.Name;
            TrackView = trackView;
            m_BottomLine.style.backgroundColor = clip.Color();

            if (clip.IsResizeable())
            {
                m_LeftResizeDragLine = new DragLineManipulator(DraglineDirection.Left, (e) =>
                {
                });
            }
        }

        public void Resize(int startFrame, int endFrame)
        {
            
        }

        public void Refresh()
        {
            
        }
        
        // public Dictionary<int,float> FramePosMap => FieldVie
        public bool IsSelectable()
        {
            throw new System.NotImplementedException();
        }

        public bool HitTest(Vector2 localPoint)
        {
            throw new System.NotImplementedException();
        }

        public void Select(VisualElement selectionContainer, bool additive)
        {
            throw new System.NotImplementedException();
        }

        public void Unselect(VisualElement selectionContainer)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSelected(VisualElement selectionContainer)
        {
            throw new System.NotImplementedException();
        }

        private void OnStartDrag(PointerDownEvent evt)
        {
            Clip.Invalid = false;
        }

        private void OnStopDrag()
        {
            Clip.Invalid = false;
        }

        private void OnDragMove(Vector2 deltaPosition)
        {
            
        }

        //TODO 2021没有这个方法
        // private void OnDrawBoxGenerateVisualContent(MeshGenerationContext)
        // {
        //     if (Hoverd)
        //     {
        //         var paint2D = mgc.
        //     }
        // }
        private static Type CopyType;
        private static Dictionary<FieldInfo, object> CopyValueMap = new();
    }
}