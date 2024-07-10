using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class MarkerView: VisualElement, ISelectable
    {
        private readonly VisualElement markerView;
        private TimelineTrackView trackView;

        public BBKeyframeBase keyframeBase;

        protected MarkerView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineMarkerView");
            visualTree.CloneTree(this);

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Loader/Timeline/Editor/Resources/Style/TimelineMarkerView.uss");
            styleSheets.Add(styleSheet);

            markerView = this.Q<VisualElement>("marker-view");
            DragManipulator dragManipulator = new(OnStartDrag, OnDragStop, OnDragMove);
            this.AddManipulator(dragManipulator);
        }

        //初始化
        public virtual void Init(TimelineTrackView _trackView, BBKeyframeBase _keyframeBase)
        {
            trackView = _trackView;
            // Add to selection
            SelectionContainer = _trackView.FieldView; 
            SelectionContainer.SelectionElements.Add(this);
            
            keyframeBase = _keyframeBase;

            Refresh();
        }

        /// <summary>
        /// 刷新marker
        /// </summary>
        public virtual void Refresh()
        {
            if (!FieldView.FramePosMap.TryGetValue(keyframeBase.frame, out float pos))
            {
                Debug.LogError("not exist frame: " + keyframeBase.frame);
                return;
            }

            style.left = pos - 6;
        }

        public virtual bool InMiddle(int targetFrame)
        {
            return keyframeBase.frame == targetFrame;
        }

        #region Event

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
            }
            else if (evt.button == 1)
            {
                
            }
        }

        protected virtual void OnStartDrag(PointerDownEvent evt)
        {
            FieldView.MarkerStartMove(this);
        }

        protected virtual void OnDragStop()
        {
            FieldView.ApplyMarkerMove();
        }

        protected virtual void OnDragMove(Vector2 movePos)
        {
            FieldView.MoveMarkers(movePos.x);
        }

        #endregion

        #region Move

        public bool InValid;

        public void Move(int deltaFrame)
        {
            keyframeBase.frame += deltaFrame;
        }

        public void ResetMove(int deltaFrame)
        {
            InValid = true;
            keyframeBase.frame -= deltaFrame;
        }

        public bool GetMoveValid()
        {
            foreach (MarkerView view in trackView.markerViews)
            {
                if (view == this)
                {
                    continue;
                }

                if (view.keyframeBase.frame == keyframeBase.frame)
                {
                    return false;
                }
            }

            return true;
        }
        
        #endregion

        #region Select

        public ISelection SelectionContainer { get; set; }
        protected TimelineFieldView FieldView => SelectionContainer as TimelineFieldView;

        private bool m_IsSelected;

        public bool IsSelectable()
        {
            return true;
        }

        public virtual void Select()
        {
            m_IsSelected = true;
            BringToFront();
            markerView.AddToClassList("Selected");
        }

        public void UnSelect()
        {
            m_IsSelected = false;
            markerView.RemoveFromClassList("Selected");
        }

        public bool IsSelected()
        {
            return m_IsSelected;
        }

        #endregion
    }
}