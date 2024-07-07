using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    [Serializable]
    public class MarkerInspectorData
    {
        [HideReferenceObjectPicker, HideLabel]
        public MarkerInfo info;

        public MarkerInspectorData(MarkerInfo target, TimelineFieldView _fieldView)
        {
            info = target;
        }
    }

    public class TimelineMarkerView: VisualElement, ISelectable
    {
        public MarkerInfo info;

        private readonly DropdownMenuHandler m_MenuHandle;
        private readonly VisualElement MarkerView;
        public bool InValid;

        public new class UxmlFactory: UxmlFactory<TimelineMarkerView, UxmlTraits>
        {
        }

        public TimelineMarkerView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineMarkerView");
            visualTree.CloneTree(this);

            StyleSheet styleSheet =
                    AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Loader/Timeline/Editor/Resources/Style/TimelineMarkerView.uss");
            styleSheets.Add(styleSheet);

            MarkerView = this.Q<VisualElement>("marker-view");
            m_MenuHandle = new DropdownMenuHandler(MenuBuilder);

            var dragManipulator = new DragManipulator(OnStartDrag, OnDragStop, OnDragMove);
            this.AddManipulator(dragManipulator);
        }

        public void Init(MarkerInfo _info)
        {
            info = _info;
            Refresh();
        }

        public void Refresh()
        {
            if (!fieldView.FramePosMap.TryGetValue(info.frame, out float pos))
            {
                Debug.LogError("not exist frame:" + info.frame);
                return;
            }

            float relativePos = pos - fieldView.ScrollViewContentOffset;
            style.left = relativePos - 6;
        }

        public bool InMiddle(int targetFrame)
        {
            return info.frame == targetFrame;
        }

        private void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Delete Marker", _ => { fieldView.EditorWindow.ApplyModify(() => { fieldView.EditorWindow.BBTimeline.Marks.Remove(info); }, "Delete Marker"); });
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

        #region Move marker

        public void Move(int deltaFrame)
        {
            info.frame += deltaFrame;
        }

        public void ResetMove(int deltaFrame)
        {
            InValid = true;
            info.frame -= deltaFrame;
        }

        private void OnStartDrag(PointerDownEvent evt)
        {
            fieldView.MarkerStartMove(this);
        }

        private void OnDragStop()
        {
            fieldView.ApplyMarkerMove();
        }

        private void OnDragMove(Vector2 movePos)
        {
            fieldView.MoveMarkers(movePos.x);
        }

        #endregion

        #region Select

        public ISelection SelectionContainer { get; set; }
        private TimelineFieldView fieldView => SelectionContainer as TimelineFieldView;

        public bool IsSelectable()
        {
            return true;
        }

        public void Select()
        {
            m_IsSelected = true;
            BringToFront();
            MarkerView.AddToClassList("Selected");
            OpenInspector();
        }

        public void UnSelect()
        {
            m_IsSelected = false;
            MarkerView.RemoveFromClassList("Selected");
        }

        private bool m_IsSelected;

        public bool IsSelected()
        {
            return this.m_IsSelected;
        }

        #endregion

        #region Inspector

        private void OpenInspector()
        {
            TimelineInspectorData.CreateView(fieldView.ClipInspector, new MarkerInspectorData(info, fieldView));
        }

        #endregion
    }
}