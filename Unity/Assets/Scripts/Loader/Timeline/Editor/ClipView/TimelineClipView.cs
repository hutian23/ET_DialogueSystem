using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineClipView: VisualElement, ISelectable, IShowInspector
    {
        public new class UxmlFactory: UxmlFactory<TimelineClipView, UxmlTraits>
        {
        }

        private bool Selected { get; set; }
        private bool Hoverd { get; set; }
        public ISelection SelectionContainer { get; set; }

        private TimelineTrackView TrackView { get; set; }
        protected TimelineFieldView FieldView => SelectionContainer as TimelineFieldView;
        protected TimelineEditorWindow EditorWindow => FieldView.EditorWindow;
        public BBClip BBClip;
        public BBTrack BBTrack => TrackView.RuntimeTrack.Track;

        protected Dictionary<int, float> FramePosMap => FieldView.FramePosMap;
        public int StartFrame => BBClip.StartFrame;
        public int EndFrame => BBClip.EndFrame;

        private DragLineManipulator m_LeftResizeDragLine;
        protected DragLineManipulator m_SelfEaseInDragLine;
        private DragLineManipulator m_RightResizeDragLine;
        protected DragLineManipulator m_SelfEaseOutDragLine;
        private readonly DropdownMenuHandler m_MenuHandle;

        protected readonly VisualElement m_Content;
        private readonly VisualElement m_Title;
        private readonly Label m_ClipName;
        private readonly VisualElement m_BottomLine;
        private readonly VisualElement m_DrawBox;

        private ShowInspectorData inspectorData;

        public TimelineClipView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineClipView");
            visualTree.CloneTree(this);
            AddToClassList("timelineClip");

            m_Content = this.Q("content");

            // m_Title = this.Q("title");
            m_ClipName = this.Q<Label>("clip-name");
            m_BottomLine = this.Q("bottom-line");
            m_DrawBox = this.Q("draw-box");

            DragManipulator mMoveDrag = new(OnStartDrag, OnStopDrag, OnDragMove);
            mMoveDrag.enabled = true;
            this.AddManipulator(mMoveDrag);

            m_MenuHandle = new DropdownMenuHandler(MenuBuilder);
            m_DrawBox.generateVisualContent += OnDrawBoxGenerateVisualContent;
        }

        public void Init(BBClip clip, TimelineTrackView trackView)
        {
            BBClip = clip;
            TrackView = trackView;

            m_ClipName.text = clip.Name;
            m_BottomLine.style.backgroundColor = ColorAttribute.GetColor(clip.GetType());

            // Resize left
            m_LeftResizeDragLine = new DragLineManipulator(DraglineDirection.Left,
                (e) =>
                {
                    FieldView.ResizeClip(this, DraglineDirection.Left, e.x);
                    FieldView.DrawFrameLine(EndFrame);
                },
                _ => { FieldView.DrawFrameLine(EndFrame); },
                () => { FieldView.DrawFrameLine(); });
            m_LeftResizeDragLine.Size = 8;
            this.AddManipulator(m_LeftResizeDragLine);

            //Resize Right
            m_RightResizeDragLine = new DragLineManipulator(DraglineDirection.Right, (e) =>
                {
                    FieldView.ResizeClip(this, DraglineDirection.Right, e.x);
                    FieldView.DrawFrameLine(StartFrame);
                },
                _ => { FieldView.DrawFrameLine(StartFrame); }, () => { FieldView.DrawFrameLine(); });
            m_RightResizeDragLine.Size = 8;
            this.AddManipulator(m_RightResizeDragLine);

            Refresh();
        }

        public void Resize(int startFrame, int endFrame)
        {
            int deltaStartFrame = startFrame - BBClip.StartFrame;
            BBClip.StartFrame += deltaStartFrame;
            BBClip.EndFrame = endFrame;
        }

        public void Move(int deltaFrame)
        {
            BBClip.StartFrame += deltaFrame;
            BBClip.EndFrame += deltaFrame;
        }

        public void ResetMove(int deltaFrame)
        {
            BBClip.InValid = false;
            BBClip.StartFrame -= deltaFrame;
            BBClip.EndFrame -= deltaFrame;
        }

        public virtual void Refresh()
        {
            style.left = FramePosMap[StartFrame];
            style.width = FramePosMap[EndFrame] - FramePosMap[StartFrame];

            if (BBClip.InValid)
            {
                AddToClassList("invalid");
            }
            else
            {
                RemoveFromClassList("invalid");
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
        }

        #endregion

        public bool InMiddle(Vector2 worldPosition)
        {
            return m_Content.worldBound.Contains(worldPosition);
        }

        public void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button == 1)
            {
                m_MenuHandle.ShowMenu(evt);
                evt.StopImmediatePropagation();
            }
            else if (evt.button == 0)
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

        protected virtual void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Remove Clip", _ => { EditorWindow.ApplyModify(() => { BBTrack.RemoveClip(BBClip); }, "Remove Clip"); });
        }

        private void OnStartDrag(PointerDownEvent evt)
        {
            FieldView.StartMove(this);
        }

        private void OnStopDrag()
        {
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

        public void InspectorAwake()
        {
            inspectorData = Activator.CreateInstance(BBClip.ShowInInpsectorType, BBClip) as ShowInspectorData;
            inspectorData.InspectorAwake(FieldView);
            TimelineInspectorData.CreateView(FieldView.ClipInspector, inspectorData);
        }

        public void InsepctorUpdate()
        {
            inspectorData.InspectorUpdate(FieldView);
        }

        public void InspectorDestroy()
        {
            inspectorData.InspectorDestroy(FieldView);
            FieldView.ClipInspector.Clear();
        }
    }
}