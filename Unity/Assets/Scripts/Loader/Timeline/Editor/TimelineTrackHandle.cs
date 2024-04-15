using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineTrackHandle: VisualElement, ISelectable
    {
        public new class UxmlFactory: UxmlFactory<TimelineTrackHandle, UxmlTraits> { }

        private TextField NameField { get; set; }
        private VisualElement Icon { get; set; }

        private TimelineTrackView TrackView { get; set; }
        public TimelineEditorWindow EditorWindow => TrackView.EditorWindow;
        private TimelineFieldView FieldView => TrackView.FieldView;
        public Track Track => TrackView.Track;
        private Timeline Timeline => Track.Timeline;

        private readonly DropdownMenuHandler MenuHandler;
        private readonly float TopOffset = 5;
        private readonly float YminOffset = -77;
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
            TrackView.OnSelected = () => { SelectionContainer.AddToSelection(this); };
            TrackView.OnUnSelected = () => { SelectionContainer.RemoveFromSelection(this); };

            style.borderLeftColor = Track.Color();

            NameField = this.Q<TextField>();
            //binding track name
            SerializedProperty serializedProperty = Timeline.SerializedTimeline.FindProperty("m_Tracks");
            serializedProperty = serializedProperty.GetArrayElementAtIndex(Timeline.Tracks.IndexOf(Track));
            NameField.bindingPath = serializedProperty.FindPropertyRelative("Name").propertyPath;
            NameField.Bind(Timeline.SerializedTimeline);

            //track Icon
            Icon = this.Q("icon");
            Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(IconGuidAttribute.Guid(Track.GetType())));
            if (texture2D)
            {
                Icon.style.backgroundImage = texture2D;
            }

            FieldView.OnGeometryChangedCallback += OnGeometryChanged;
            RegisterCallback<GeometryChangedEvent>( _ => OnGeometryChanged());
            RegisterCallback<DetachFromPanelEvent>( _ => FieldView.OnGeometryChangedCallback -= OnGeometryChanged);

            MenuHandler = new DropdownMenuHandler(MenuBuilder);
            DragManipulator = new DragManipulator((e) =>
            {
                //OnDrag
                Dragging = true;
                OriginalIndex = Timeline.Tracks.IndexOf(Track); //交换前的Index
                e.StopImmediatePropagation();
            }, () =>
            {
                //OnDrop
                Dragging = false;
                Tweening = false;
                EditorApplication.update -= TweenTrackHandles;
                
                int currentIndex = Timeline.Tracks.IndexOf(Track);
                Timeline.Tracks.Remove(Track);
                Timeline.Tracks.Insert(OriginalIndex,Track);
                
                if (OriginalIndex != currentIndex)
                {
                    Timeline.ApplyModify(() =>
                    {
                        Timeline.Tracks.Remove(Track);
                        Timeline.Tracks.Insert(currentIndex,Track);
                        Timeline.Resort();
                    },"Resort");
                }
            }, (e) =>
            {
                //OnMove
                float targetY = transform.position.y + e.y;
                targetY = Mathf.Clamp(targetY, TopOffset, (Timeline.Tracks.Count - 1) * Interval + TopOffset);
                transform.position = new Vector3(0, targetY, 0);
                TrackView.transform.position = new Vector3(0, targetY - TopOffset, 0); //<---TrackView的联动效果
                
                int index = Timeline.Tracks.IndexOf(Track);
                int targetIndex = Mathf.FloorToInt(targetY / Interval);
                if (index != targetIndex)
                {
                    Timeline.Tracks.Remove(Track);
                    Timeline.Tracks.Insert(targetIndex,Track);
                }
                
                if (!Tweening)
                {
                    EditorApplication.update += TweenTrackHandles;
                }
            });
            this.AddManipulator(DragManipulator);
        }

        private void OnGeometryChanged()
        {
            transform.position = new Vector3(0, TrackView.worldBound.yMin + YminOffset, 0);
            Debug.LogWarning(transform.position);
        }

        private void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Add Clip", _ => { Timeline.ApplyModify(() => { FieldView.AddClip(Track, FieldView.GetRightEdgeFrame(Track)); }, "Add Clip"); });
            menu.AppendAction("Remove Track", _ => { Timeline.ApplyModify(() => { Timeline.RemoveTrack(Track); }, "Remove Track"); });
            menu.AppendAction("Mute Track", _ =>
            {
                Timeline.ApplyModify(() => { Track.PersistentMuted = !Track.PersistentMuted; }, "Mute Track");
                Timeline.RebindAll();
            }, _ => Track.PersistentMuted? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);
            menu.AppendAction("Open Script", _ => { Track.OpenTrackScript(); });
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
                    }
                }
                else
                {
                    if (evt.actionKey)
                    {
                        FieldView.RemoveFromSelection(this);
                    }
                }
                DragManipulator.DragBeginForce(evt,this.WorldToLocal(evt.position));
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
                var bindingPath = Regex.Replace(trackHandle.NameField.bindingPath,
                    @"(m_Tracks.Array.data\[)(\d+)(\].Name)",
                    "m_Tracks.Array.data[" + Timeline.Tracks.IndexOf(trackHandle.Track) + "].Name");
                trackHandle.NameField.bindingPath = bindingPath;
                trackHandle.NameField.Bind(Timeline.SerializedTimeline);

                if (!trackHandle.Dragging)
                {
                    float targetY = Timeline.Tracks.IndexOf(trackHandle.Track) * Interval + TopOffset;
                    float currentY = trackHandle.transform.position.y;
                    if (Mathf.Abs(currentY - targetY) > 1f)
                    {
                        Tweening = true;
                        targetY = Mathf.Lerp(currentY, targetY, 0.05f);
                    }

                    trackHandle.transform.position = new Vector3(0, targetY, 0);
                    trackHandle.TrackView.transform.position = new Vector3(0, targetY - TopOffset, 0);
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