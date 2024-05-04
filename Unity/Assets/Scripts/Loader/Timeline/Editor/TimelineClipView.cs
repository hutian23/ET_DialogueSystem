using System;
using System.Collections.Generic;
using System.Reflection;
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
        private BBClip BBClip;

        private Timeline Timeline => EditorWindow.Timeline;
        private Dictionary<int, float> FramePosMap => FieldView.FramePosMap;
        public Clip Clip { get; private set; }
        public TimelineTrackView TrackView { get; private set; }

        public int StartFrame => BBClip.StartFrame;
        public int EndFrame => BBClip.EndFrame;
        private int WidthFrame => EndFrame - StartFrame;

        private DragLineManipulator m_LeftResizeDragLine;
        protected DragLineManipulator m_SelfEaseInDragLine;
        private DragLineManipulator m_RightResizeDragLine;
        protected DragLineManipulator m_SelfEaseOutDragLine;
        private readonly DragManipulator m_MoveDrag;
        private readonly DropdownMenuHandler m_MenuHandle;

        private readonly VisualElement m_Content;
        private readonly VisualElement m_Title;
        private readonly Label m_ClipName;
        private readonly VisualElement m_BottomLine;
        private readonly VisualElement m_DrawBox;

        public TimelineClipView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineClipView");
            visualTree.CloneTree(this);
            AddToClassList("timelineClip");

            m_Content = this.Q("content");

            m_Title = this.Q("title");
            m_ClipName = this.Q<Label>("clip-name");
            m_BottomLine = this.Q("bottom-line");
            m_DrawBox = this.Q("draw-box");

            // m_MoveDrag = new DragManipulator(OnStartDrag, OnStopDrag, OnDragMove);
            // m_MoveDrag.enabled = true;
            // this.AddManipulator(m_MoveDrag);
            //
            // m_MenuHandle = new DropdownMenuHandler(MenuBuilder);
            m_DrawBox.generateVisualContent += OnDrawBoxGenerateVisualContent;
        }

        public void Init(BBClip clip, TimelineTrackView trackView)
        {
            BBClip = clip;
            TrackView = trackView;

            m_ClipName.text = clip.Name;
            m_BottomLine.style.backgroundColor = ColorAttribute.GetColor(clip.GetType());

            Refresh();
        }

        public void Init(Clip clip, TimelineTrackView trackView)
        {
            // Clip = clip;
            // Clip.OnNameChanged = () => m_ClipName.text = clip.Name;
            // m_ClipName.text = clip.Name;
            // TrackView = trackView;
            // // m_BottomLine.style.backgroundColor = clip.Color();
            //
            // //Resize
            // m_LeftResizeDragLine = new DragLineManipulator(DraglineDirection.Left, (e) =>
            // {
            //     FieldView.ResizeClip(this, DraglineDirection.Left, e.x);
            //     FieldView.DrawFrameLine(EndFrame);
            // }, _ =>
            // {
            //     FieldView.DrawFrameLine(EndFrame);
            // }, () =>
            // {
            //     FieldView.DrawFrameLine();
            // });
            // m_LeftResizeDragLine.Size = 8;
            // this.AddManipulator(m_LeftResizeDragLine);
            //
            // m_RightResizeDragLine = new DragLineManipulator(DraglineDirection.Right, (e) =>
            // {
            //     FieldView.ResizeClip(this, DraglineDirection.Right, e.x);
            //     // if (!IsSelected())
            //     // {
            //     //     SelectionContainer.ClearSelection();
            //     //     SelectionContainer.AddToSelection(this);
            //     // }
            //     FieldView.DrawFrameLine(StartFrame);
            // }, _ =>
            // {
            //     FieldView.DrawFrameLine(StartFrame);
            // }, () =>
            // {
            //     FieldView.DrawFrameLine();
            // });
            // m_RightResizeDragLine.Size = 8;
            // this.AddManipulator(m_RightResizeDragLine);
            //
            // Refresh();
        }

        public void Resize(int startFrame, int endFrame)
        {
            int deltaStartFrame = startFrame - Clip.StartFrame;
            Clip.StartFrame += deltaStartFrame;
            Clip.EndFrame = endFrame;
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
            // if (Clip.Invalid)
            // {
            //     AddToClassList("invalid");
            // }
            // else
            // {
            //     RemoveFromClassList("invalid");
            // }
            //
            // if (Clip.Invalid)
            // {
            //     m_Content.style.left = 0;
            //     m_Content.style.width = m_Title.style.width = (WidthFrame * FieldView.OneFrameWidth);
            // }
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
                m_MoveDrag.DragBeginForce(evt, this.WorldToLocal(evt.position));
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
            else if (!value && Hoverd)
            {
                Hoverd = false;
                Add(m_DrawBox);
                m_DrawBox.MarkDirtyRepaint();
            }
        }

        private void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Remove Clip", _ => { Timeline.ApplyModify(() => { Timeline.RemoveClip(Clip); }, "Remove Clip"); });
            menu.AppendAction("Open Script", _ =>
            {
                //TODO Open Script
            });
            menu.AppendAction("Paste Properties", _ =>
            {
                foreach (var fieldInfo in Clip.GetAllFields())
                {
                    if (fieldInfo.GetCustomAttribute<ShowInInspectorAttribute>() != null && CopyValueMap.TryGetValue(fieldInfo, out object value))
                    {
                        CopyValueMap.Add(fieldInfo, fieldInfo.GetValue(Clip));
                    }
                }
            }, _ =>
            {
                if (CopyType == null)
                {
                    return DropdownMenuAction.Status.None;
                }
                else if (CopyType != Clip.GetType())
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
                paint2D.MoveTo(new Vector2(0, 0));
                paint2D.LineTo(new Vector2(worldBound.width, 0));
                paint2D.LineTo(new Vector2(worldBound.width, worldBound.height));
                paint2D.LineTo(new Vector2(0, worldBound.height));
                paint2D.LineTo(new Vector2(0, 0));
                paint2D.Stroke();
            }
        }

        private static Type CopyType;
        private static readonly Dictionary<FieldInfo, object> CopyValueMap = new();
    }
}