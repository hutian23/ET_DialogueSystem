using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineClipView : VisualElement,ISelectable
    {
        public new class UxmlFactory : UxmlFactory<TimelineClipView,UxmlTraits> {}

        protected string m_DefaultVisualTreeGuid = "";
        protected virtual string VisualTreeGuid => m_DefaultVisualTreeGuid;
        
        public bool Selected { get; private set; }
        public bool Hoverd { get; private set; }
        public ISelection SelectionContainer { get; set; }
        public ClipCapabilities Capabilities => Clip.Capabilities;

        public TimelineFieldView FieldView => SelectionContainer as TimelineFieldView;
        public TimelineEditorWindow EditorWindow => FieldView.EditorWindow;
        public Timeline Timeline => EditorWindow.Timeline;
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
            
            // m_MenuHandle = new DropdownMenuHandler(MenuBu)
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
                    FieldView.ResizeClip(this,0,e.x);
                    if (!IsSelected())
                    {
                        SelectionContainer.ClearSelection();
                        SelectionContainer.AddToSelection(this);
                    }
                    FieldView.DrawFrameLine(StartFrame);
                }, (e) =>
                {
                    FieldView.DrawFrameLine(StartFrame);
                }, () =>
                {
                    FieldView.DrawFrameLine();
                });
                m_LeftResizeDragLine.Size = 4;
                this.AddManipulator(m_LeftResizeDragLine);

                m_RightResizeDragLine = new DragLineManipulator(DraglineDirection.Right, (e) =>
                {
                    FieldView.ResizeClip(this,1,e.x);
                    if (!IsSelected())
                    {
                        SelectionContainer.ClearSelection();
                        SelectionContainer.AddToSelection(this);
                    }
                    FieldView.DrawFrameLine(StartFrame);
                }, (e) =>
                {
                    FieldView.DrawFrameLine(StartFrame);
                }, () =>
                {
                    FieldView.DrawFrameLine();
                });
                m_RightResizeDragLine.Size = 4;
                this.AddManipulator(m_RightResizeDragLine);
            }

            if (clip.IsMixable())
            {
                m_SelfEaseInDragLine = new DragLineManipulator(DraglineDirection.Right, (e) =>
                {
                    FieldView.AdjustSelfEase(this,0,e.x);
                    FieldView.DrawFrameLine(StartFrame + SelfEaseInFrame);
                }, (e) =>
                {
                    FieldView.DrawFrameLine(StartFrame + SelfEaseInFrame);
                }, () =>
                {
                    FieldView.DrawFrameLine();
                });
                m_SelfEaseInDragLine.Size = 4;
                m_LeftMixer.AddManipulator(m_SelfEaseInDragLine);
                SelfEaseIn = false;

                m_SelfEaseOutDragLine = new DragLineManipulator(DraglineDirection.Left, (e) =>
                {
                    FieldView.AdjustSelfEase(this,1,e.x);
                    FieldView.DrawFrameLine(EndFrame - SelfEaseOutFrame);
                }, (e) =>
                {
                    FieldView.DrawFrameLine(EndFrame - SelfEaseOutFrame);
                }, () =>
                {
                    FieldView.DrawFrameLine();
                });
                m_SelfEaseOutDragLine.Size = 4;
                m_RightMixer.AddManipulator(m_SelfEaseOutDragLine);
                SelfEaseOut = false;
            }
            
            Refresh();
        }

        public void Resize(int startFrame, int endFrame)
        {
            int deltaStartFrame = startFrame - Clip.StartFrame;
            float easeInRatio = (float)Clip.SelfEaseInFrame / Clip.Duration;
            float easeOutRatio = (float)Clip.SelfEaseOutFrame / Clip.Duration;

            Clip.StartFrame += deltaStartFrame;
            if (Clip.IsClipInable())
            {
                Clip.ClipInFrame += deltaStartFrame;
            }

            Clip.EndFrame = endFrame;

            Clip.SelfEaseInFrame = Mathf.RoundToInt(easeInRatio * Clip.Duration);
            Clip.SelfEaseOutFrame = Mathf.Min(Mathf.RoundToInt(easeOutRatio * Clip.Duration), Clip.Duration - SelfEaseInFrame);
            
            Clip.Track.UpdateMix();
        }

        public void AdjustSelfEase(int border, int deltaFrame)
        {
            if (border == 0)
            {
                Clip.SelfEaseInFrame += deltaFrame;
            }
            else
            {
                Clip.SelfEaseOutFrame -= deltaFrame;
            }
            Clip.Track.UpdateMix();
        }

        public void Move(int deltaFrame)
        {
            Clip.StartFrame += deltaFrame;
            Clip.EndFrame += deltaFrame;
        }
        
        public void Refresh()
        {
            
        }
        
        // public Dictionary<int,float> FramePosMap => FieldVie
        public bool IsSelectable()
        {
            throw new System.NotImplementedException();
        }

        public void Select()
        {
            throw new NotImplementedException();
        }

        public void UnSelect()
        {
            throw new NotImplementedException();
        }

        public bool IsSelected()
        {
            throw new NotImplementedException();
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