using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineTrackHandle: VisualElement, ISelectable
    {
        public new class UxmlFactory: UxmlFactory<TimelineTrackHandle, UxmlTraits>
        {
        }

        private Label NameLabel { get; set; }
        private TextField NameField { get; set; }
        private VisualElement Icon { get; set; }
        private VisualElement eyeBtn { get; set; }
        private VisualElement eyeCloseBtn { get; set; }
        private TimelineTrackView TrackView { get; set; }
        private BBTrack BBTrack => TrackView.Track;
        private BBTimeline BBTimeline => EditorWindow.BBTimeline;
        private RuntimePlayable RuntimePlayable => EditorWindow.RuntimePlayable;

        private TimelineEditorWindow EditorWindow => TrackView.EditorWindow;
        private TimelineFieldView FieldView => TrackView.FieldView;

        private readonly DropdownMenuHandler MenuHandler;

        private readonly float Interval = 40; //TrackHandle_Height + margin_Top + margin_Bottom

        public TimelineTrackHandle()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineTrackHandle");
            visualTree.CloneTree(this);
            AddToClassList("timelineTrackHandle");
            pickingMode = PickingMode.Ignore;
        }

        public TimelineTrackHandle(TimelineTrackView trackView): this()
        {
            TrackView = trackView;

            style.borderLeftColor = ColorAttribute.GetColor(BBTrack.GetType());

            //track name
            NameLabel = this.Q<Label>();
            NameLabel.pickingMode = PickingMode.Ignore;
            NameField = this.Q<TextField>();
            NameField.pickingMode = PickingMode.Ignore;
            //因为用了odinSerialized 这里无法反射获取属性
            NameField.RegisterCallback<BlurEvent>(_ =>
            {
                if (EditorWindow.BBTimeline.ContainTrack(NameField.value))
                {
                    RefreshEditName(false);
                    return;
                }

                EditorWindow.ApplyModify(() => { BBTrack.Name = NameField.value; }, "Rename BBTrack");
            });
            RefreshEditName(false);

            transform.position = new Vector3(0, GetTrackOrder() * 40, 0);

            //track Icon
            Icon = this.Q("icon");
            Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(IconGuidAttribute.Guid(BBTrack.GetType())));
            if (texture2D)
            {
                Icon.style.backgroundImage = texture2D;
            }

            MenuHandler = new DropdownMenuHandler(MenuBuilder);

            DragManipulator = new DragManipulator((e) =>
            {
                //OnDrag
                Dragging = true;
                OriginalIndex = GetTrackIndex(); //交换前的Index
                e.StopImmediatePropagation();
            }, () =>
            {
                //OnDrop
                Dragging = false;
                Tweening = false;
                EditorApplication.update -= TweenTrackHandles;

                int currentIndex = GetTrackIndex();
                BBTimeline.Tracks.Remove(BBTrack);
                BBTimeline.Tracks.Insert(OriginalIndex, BBTrack);

                if (OriginalIndex != currentIndex)
                {
                    //Undo
                    EditorWindow.ApplyModify(() =>
                    {
                        BBTimeline.Tracks.Remove(BBTrack);
                        BBTimeline.Tracks.Insert(currentIndex, BBTrack);
                    }, "Resort");
                }
                //移动了但是没有完全移动
                else
                {
                    float targetY = Interval * currentIndex;
                    transform.position = new Vector3(0, targetY, 0);
                    TrackView.transform.position = new Vector3(0, targetY, 0);
                }
            }, (e) =>
            {
                //OnMove
                float targetY = transform.position.y + e.y;
                targetY = Mathf.Clamp(targetY, 0, (BBTimeline.Tracks.Count - 1) * Interval);
                transform.position = new Vector3(0, targetY, 0);
                TrackView.transform.position = new Vector3(0, targetY, 0); //<---TrackView的联动效果

                int index = GetTrackIndex();
                int targetIndex = Mathf.RoundToInt(targetY / Interval);
                if (index != targetIndex)
                {
                    BBTimeline.Tracks.Remove(BBTrack);
                    BBTimeline.Tracks.Insert(targetIndex, BBTrack);
                }

                if (!Tweening)
                {
                    EditorApplication.update += TweenTrackHandles;
                }
            });
            this.AddManipulator(DragManipulator);

            //Enable
            eyeBtn = this.Q("eye");
            eyeBtn.style.display = BBTrack.Enable? DisplayStyle.Flex : DisplayStyle.None;
            eyeCloseBtn = this.Q("eye-close");
            eyeCloseBtn.style.display = BBTrack.Enable? DisplayStyle.None : DisplayStyle.Flex;
            eyeBtn.RegisterCallback<PointerDownEvent>(_ => { EditorWindow.ApplyModify(() => { BBTrack.Enable = false; }, "Disable Track"); });
            eyeCloseBtn.RegisterCallback<PointerDownEvent>(_ => { EditorWindow.ApplyModify(() => { BBTrack.Enable = true; }, "Enable Track"); });
        }

        //下面这两个其实一样
        private int GetTrackIndex()
        {
            return EditorWindow.BBTimeline.Tracks.IndexOf(BBTrack);
        }

        private int GetTrackOrder()
        {
            return FieldView.TrackViews.IndexOf(TrackView);
        }

        private void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Remove Track", _ => { EditorWindow.ApplyModify(() => { RuntimePlayable.RemoveTrack(BBTrack); }, "Remove Track"); });
            menu.AppendAction("Edit Track Name", _ => { RefreshEditName(true); });
        }

        public void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button == 0 && IsSelectable())
            {
                if (!IsSelected())
                {
                    if (evt.actionKey)
                    {
                        FieldView.AddToSelection(TrackView);
                    }
                    else
                    {
                        FieldView.ClearSelection();
                        FieldView.AddToSelection(TrackView);
                        FieldView.AddToSelection(this);
                    }
                }
                else
                {
                    if (evt.actionKey)
                    {
                        FieldView.RemoveFromSelection(this);
                        FieldView.RemoveFromSelection(TrackView);
                    }
                }

                DragManipulator.DragBeginForce(evt, this.WorldToLocal(evt.position));
                evt.StopImmediatePropagation();
            }
            else if (evt.button == 1)
            {
                FieldView.ClearSelection();
                FieldView.AddToSelection(TrackView);
                MenuHandler.ShowMenu(evt);
                evt.StopImmediatePropagation();
            }
        }

        private void RefreshEditName(bool editMode)
        {
            NameField.style.display = editMode? DisplayStyle.Flex : DisplayStyle.None;
            NameLabel.style.display = editMode? DisplayStyle.None : DisplayStyle.Flex;

            NameLabel.text = BBTrack.Name;
            NameField.value = BBTrack.Name;

            if (editMode)
            {
                NameField.Focus();
            }
        }

        #region Drag

        private bool Dragging;
        private int OriginalIndex;
        private readonly DragManipulator DragManipulator;
        private static bool Tweening;

        private void TweenTrackHandles()
        {
            Tweening = false;
            EditorApplication.update -= TweenTrackHandles;

            //交换之后每个trackHandle都要重新绑定一次
            var trackHandles = parent.Query<TimelineTrackHandle>().ToList();
            foreach (var trackHandle in trackHandles)
            {
                if (!trackHandle.Dragging)
                {
                    float targetY = trackHandle.GetTrackIndex() * Interval;
                    float currentY = trackHandle.transform.position.y;
                    if (Mathf.Abs(currentY - targetY) > 1f)
                    {
                        Tweening = true;
                        targetY = Mathf.Lerp(currentY, targetY, 0.05f);
                    }

                    trackHandle.transform.position = new Vector3(0, targetY, 0);
                    trackHandle.TrackView.transform.position = new Vector3(0, targetY, 0);
                }
            }

            if (Tweening)
            {
                EditorApplication.update += TweenTrackHandles;
            }
        }

        #endregion

        #region Selectable

        public bool Selected { get; private set; }
        public ISelection SelectionContainer { get; set; }

        public override bool Overlaps(Rect rectangle)
        {
            return false;
        }

        public bool IsSelectable()
        {
            return true;
        }

        public bool IsSelected()
        {
            return TrackView.IsSelected();
        }

        public void Select()
        {
            AddToClassList("selected");
            BringToFront();
        }

        public void UnSelect()
        {
            RemoveFromClassList("selected");
        }

        #endregion
    }
}