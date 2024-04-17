using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineClipView: VisualElement, ISelectable
    {
        public new class UxmlFactory: UxmlFactory<TimelineClipView, UxmlTraits>
        {
        }

        private bool Selected { get; set; }
        private bool Hoverd { get; set; }
        public ISelection SelectionContainer { get; set; }
        public ClipCapabilities Capabilities => Clip.Capabilities;

        private TimelineFieldView FieldView => SelectionContainer as TimelineFieldView;
        private TimelineEditorWindow EditorWindow => FieldView.EditorWindow;
        private Timeline Timeline => EditorWindow.Timeline;
        private Dictionary<int, float> FramePosMap => FieldView.FramePosMap;
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
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineClipView");
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
            m_MoveDrag.enabled = true;
            this.AddManipulator(m_MoveDrag);

            m_MenuHandle = new DropdownMenuHandler(MenuBuilder);
            m_DrawBox.generateVisualContent += OnDrawBoxGenerateVisualContent;
        }

        public void Init(Clip clip, TimelineTrackView trackView)
        {
            Clip = clip;
            Clip.OnNameChanged = () => m_ClipName.text = clip.Name;
            m_ClipName.text = clip.Name;
            
            TrackView = trackView;
            m_BottomLine.style.backgroundColor = clip.Color();

            // if (clip.IsResizeable())
            // {
            //     m_LeftResizeDragLine = new DragLineManipulator(DraglineDirection.Left, (e) =>
            //     {
            //         FieldView.ResizeClip(this, 0, e.x);
            //         if (!IsSelected())
            //         {
            //             SelectionContainer.ClearSelection();
            //             SelectionContainer.AddToSelection(this);
            //         }
            //
            //         FieldView.DrawFrameLine(StartFrame);
            //     }, (e) => { FieldView.DrawFrameLine(StartFrame); }, () => { FieldView.DrawFrameLine(); });
            //     m_LeftResizeDragLine.Size = 4;
            //     this.AddManipulator(m_LeftResizeDragLine);
            //
            //     m_RightResizeDragLine = new DragLineManipulator(DraglineDirection.Right, (e) =>
            //     {
            //         FieldView.ResizeClip(this, 1, e.x);
            //         if (!IsSelected())
            //         {
            //             SelectionContainer.ClearSelection();
            //             SelectionContainer.AddToSelection(this);
            //         }
            //
            //         FieldView.DrawFrameLine(StartFrame);
            //     }, (e) => { FieldView.DrawFrameLine(StartFrame); }, () => { FieldView.DrawFrameLine(); });
            //     m_RightResizeDragLine.Size = 4;
            //     this.AddManipulator(m_RightResizeDragLine);
            // }
            
            // if (clip.IsMixable())
            // {
            //     m_SelfEaseInDragLine = new DragLineManipulator(DraglineDirection.Right, (e) =>
            //     {
            //         FieldView.AdjustSelfEase(this, 0, e.x);
            //         FieldView.DrawFrameLine(StartFrame + SelfEaseInFrame);
            //     }, (e) => { FieldView.DrawFrameLine(StartFrame + SelfEaseInFrame); }, () => { FieldView.DrawFrameLine(); });
            //     m_SelfEaseInDragLine.Size = 4;
            //     m_LeftMixer.AddManipulator(m_SelfEaseInDragLine);
            //     SelfEaseIn = false;
            //
            //     m_SelfEaseOutDragLine = new DragLineManipulator(DraglineDirection.Left, (e) =>
            //     {
            //         FieldView.AdjustSelfEase(this, 1, e.x);
            //         FieldView.DrawFrameLine(EndFrame - SelfEaseOutFrame);
            //     }, (e) => { FieldView.DrawFrameLine(EndFrame - SelfEaseOutFrame); }, () => { FieldView.DrawFrameLine(); });
            //     m_SelfEaseOutDragLine.Size = 4;
            //     m_RightMixer.AddManipulator(m_SelfEaseOutDragLine);
            //     SelfEaseOut = false;
            // }

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

        public void ResetMove(int deltaFrame)
        {
            Clip.Invalid = false;
            Clip.StartFrame -= deltaFrame;
            Clip.EndFrame -= deltaFrame;
        }

        public void Refresh()
        {
            style.left = FramePosMap[StartFrame];
            style.width = FramePosMap[EndFrame] - FramePosMap[StartFrame];

            m_LeftClipIn.style.display = Clip.ClipInFrame > 0? DisplayStyle.Flex : DisplayStyle.None;
            m_RightClipIn.style.display = (Clip.ClipInFrame + WidthFrame) < Clip.Length? DisplayStyle.Flex : DisplayStyle.None;

            if (Clip.Invalid)
            {
                AddToClassList("invalid");
            }
            else
            {
                RemoveFromClassList("invalid");
            }

            if (EaseInFrame > 0)
            {
                AddToClassList("mixLeft");
            }
            else
            {
                RemoveFromClassList("mixLeft");
            }

            if (EaseOutFrame > 0)
            {
                AddToClassList("mixRight");
            }
            else
            {
                RemoveFromClassList("mixRight");
            }

            if (OtherEaseInFrame > 0)
            {
                SelfEaseIn = false;
            }

            if (OtherEaseOutFrame > 0)
            {
                SelfEaseOut = false;
            }

            if (Clip.Invalid)
            {
                m_Content.style.left = 0;
                m_Content.style.width = m_Title.style.width = (WidthFrame * FieldView.OneFrameWidth);
                m_LeftMixer.style.width = m_RightMixer.style.width = 0;
            }
            else
            {
                int offset = OtherEaseInFrame > 0? (OtherEaseOutFrame > 0? 0 : -2) : 0;

                float left = OtherEaseInFrame > 0? (float)OtherEaseInFrame / 2 * FieldView.OneFrameWidth * offset : 0;
                m_Content.style.left = left;

                m_Title.style.width = (WidthFrame - EaseInFrame - EaseOutFrame) * FieldView.OneFrameWidth;

                float leftWidth = (OtherEaseInFrame > 0? (float)OtherEaseInFrame / 2 : Clip.SelfEaseInFrame) * FieldView.OneFrameWidth;
                m_LeftMixer.style.width = leftWidth;

                float rightWidth = (OtherEaseOutFrame > 0? (float)OtherEaseOutFrame / 2 : Clip.SelfEaseOutFrame) * FieldView.OneFrameWidth;
                m_RightMixer.style.width = rightWidth;
            }
        }

        #region Selectable

        public virtual bool IsSelectable()
        {
            return true;
        }

        public bool IsSelected()
        {
            return Selected;
        }

        public void Select()
        {
            Selected = true;
            BringToFront();
            AddToClassList("selected");
            
            OnHover(false);
            m_DrawBox.MarkDirtyRepaint();
        }
        
        public void UnSelect()
        {
            Selected = false;
            RemoveFromClassList("selected");
            m_DrawBox.MarkDirtyRepaint();
            m_MoveDrag.enabled = false;
        }
        #endregion

        public bool InMiddle(Vector2 worldPosition)
        {
            return m_Content.worldBound.Contains(worldPosition);
        }

        public void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button == 0)
            {
                if (!IsSelected())
                {
                    if (evt.actionKey)
                    {
                        SelectionContainer.AddToSelection(this);
                    }
                    else
                    {
                        SelectionContainer.ClearSelection();
                        SelectionContainer.AddToSelection(this);
                    }
                }
                else
                {
                    if (evt.actionKey)
                    {
                        SelectionContainer.RemoveFromSelection(this);
                    }
                }

                m_MoveDrag.enabled = true;
                m_MoveDrag.DragBeginForce(evt,this.WorldToLocal(evt.position));
            }
            else if (evt.button == 1)
            {
                m_MenuHandle.ShowMenu(evt);
                evt.StopImmediatePropagation();
            }
        }
        
        public void OnHover(bool value)
        {
            if (value && !Hoverd && !Selected)
            {
                Hoverd = true;
                parent.Add(m_DrawBox);
                m_DrawBox.style.left = style.left;
                m_DrawBox.style.width = style.width;
                m_DrawBox.MarkDirtyRepaint();
            }
            else if(!value && Hoverd)
            {
                Hoverd = false;
                Add(m_DrawBox);
                m_DrawBox.MarkDirtyRepaint();
            }
        }

        private void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Adjust Self Ease In", (e) =>
            {
                SelfEaseIn = !SelfEaseIn;
            }, (e) =>
            {
                if (!Clip.IsMixable())
                {
                    return DropdownMenuAction.Status.None;
                }
                else if (OtherEaseInFrame > 0)
                {
                    return DropdownMenuAction.Status.Disabled;
                }
                else if (SelfEaseIn)
                {
                    return DropdownMenuAction.Status.Checked;
                }
                else
                {
                    return DropdownMenuAction.Status.Normal;
                }
            });
            
            menu.AppendAction("Adjust Self Ease Out", (e) =>
            {
                SelfEaseOut = !SelfEaseOut;
            }, (e) =>
            {
                if (!Clip.IsMixable())
                {
                    return DropdownMenuAction.Status.None;
                }
                else if (OtherEaseOutFrame > 0)
                {
                    return DropdownMenuAction.Status.Disabled;
                }
                else if (SelfEaseOut)
                {
                    return DropdownMenuAction.Status.Checked;
                }
                else
                {
                    return DropdownMenuAction.Status.Normal;
                }
            });
            
            menu.AppendAction("Remove Clip", (e) =>
            {
                Timeline.ApplyModify(() =>
                {
                    Timeline.RemoveClip(Clip);
                },"Remove Clip");
            });
            menu.AppendAction("Open Script", (e) =>
            {
                //TODO Open Script
            });
            menu.AppendAction("Paste Properties", (e) =>
            {
                foreach (var fieldInfo in Clip.GetAllFields())
                {
                    if (fieldInfo.GetCustomAttribute<ShowInInspectorAttribute>() != null && CopyValueMap.TryGetValue(fieldInfo, out object value))
                    {
                        CopyValueMap.Add(fieldInfo,fieldInfo.GetValue(Clip));
                    }
                }
            }, (e) =>
            {
                if (CopyType == null)
                {
                    return DropdownMenuAction.Status.None;
                }
                else if(CopyType != Clip.GetType())
                {
                    return DropdownMenuAction.Status.Disabled;
                }
                else
                {
                    return DropdownMenuAction.Status.Normal;
                }
            });
        }

        private void OnStartDrag(PointerDownEvent evt)
        {
            Clip.Invalid = false;
            FieldView.StartMove(this);
        }

        private void OnStopDrag()
        {
            Clip.Invalid = false;
            FieldView.ApplyMove();
        }

        private void OnDragMove(Vector2 deltaPosition)
        {
            FieldView.MoveClips(deltaPosition.x);
        }

        private void OnDrawBoxGenerateVisualContent(MeshGenerationContext mgc)
        {
            if (Hoverd)
            {
                var paint2D = mgc.painter2D;
                paint2D.strokeColor = new Color(68, 192, 255, 255);
                paint2D.BeginPath();
                paint2D.MoveTo(new Vector2(0,0));
                paint2D.LineTo(new Vector2(worldBound.width,0));
                paint2D.LineTo(new Vector2(worldBound.width,worldBound.height));
                paint2D.LineTo(new Vector2(0,worldBound.height));
                paint2D.Stroke();
            }
        }
        
        private static Type CopyType;
        private static Dictionary<FieldInfo, object> CopyValueMap = new();
    }
}