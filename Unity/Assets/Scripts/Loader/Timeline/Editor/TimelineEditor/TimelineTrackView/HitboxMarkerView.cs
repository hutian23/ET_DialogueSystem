using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class HitboxMarkerView: VisualElement, ISelectable
    {
        private readonly VisualElement MarkerView;
        private HitboxTrackView trackView;

        public HitboxKeyframe keyframe;

        public HitboxMarkerView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineMarkerView");
            visualTree.CloneTree(this);

            StyleSheet styleSheet =
                    AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Loader/Timeline/Editor/Resources/Style/TimelineMarkerView.uss");
            styleSheets.Add(styleSheet);

            MarkerView = this.Q<VisualElement>("marker-view");

            var dragManipulator = new DragManipulator(OnStartDrag, OnDragStop, OnDragMove);
            this.AddManipulator(dragManipulator);
        }

        public void Init(HitboxTrackView _trackView, HitboxKeyframe _keyframe)
        {
            trackView = _trackView;
            SelectionContainer = _trackView.FieldView;
            keyframe = _keyframe;
            Refresh();
        }

        public void Refresh()
        {
            if (!fieldView.FramePosMap.TryGetValue(keyframe.frame, out float pos))
            {
                Debug.LogError("not exist frame: " + keyframe.frame);
                return;
            }

            // var relativePos = pos - fieldView.ScrollViewContentOffset;
            var relativePos = pos;
            style.left = relativePos - 6;
        }

        public bool InMiddle(int targetFrame)
        {
            return keyframe.frame == targetFrame;
        }

        public void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button == 1)
            {
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
            }
            else
            {
                if (evt.actionKey)
                {
                    SelectionContainer.RemoveFromSelection(this);
                }
            }
        }

        #region Move marker

        public bool InValid;

        public void Move(int deltaFrame)
        {
            keyframe.frame += deltaFrame;
        }

        public void ResetMove(int deltaFrame)
        {
            InValid = true;
            keyframe.frame -= deltaFrame;
        }

        private void OnStartDrag(PointerDownEvent evt)
        {
            trackView.MarkerStartMove(this);
        }

        private void OnDragStop()
        {
            trackView.ApplyMarkerMove();
        }

        private void OnDragMove(Vector2 movePos)
        {
            trackView.MoveMarkers(movePos.x);
        }

        #endregion

        #region Select

        public ISelection SelectionContainer { get; set; }
        private TimelineFieldView fieldView => SelectionContainer as TimelineFieldView;

        private bool m_IsSelected;

        public bool IsSelectable()
        {
            return true;
        }

        public void Select()
        {
            m_IsSelected = true;
            BringToFront();
            MarkerView.AddToClassList("Selected");
        }

        public void UnSelect()
        {
            m_IsSelected = false;
            MarkerView.RemoveFromClassList("Selected");
        }

        public bool IsSelected()
        {
            return m_IsSelected;
        }

        #endregion
    }
}