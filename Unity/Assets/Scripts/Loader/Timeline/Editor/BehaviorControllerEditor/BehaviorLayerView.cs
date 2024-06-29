using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class BehaviorLayerView: VisualElement
    {
        public new class UxmlFactory: UxmlFactory<BehaviorLayerView, UxmlTraits>
        {
        }

        public BehaviorLayerView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/BehaviorLayerView");
            visualTree.CloneTree(this);
            AddToClassList("behaviorLayerView");

            layerText = this.Q<TextField>("layer-text");
            layerLabel = this.Q<Label>("layer-label");
        }

        private DropdownMenuManipulator MenuManipulator;
        private BehaviorControllerEditor controllerEditor;
        private ScrollView LayerViewsContainer => controllerEditor.layerViewsContainer;
        private readonly TextField layerText;
        private readonly Label layerLabel;
        private readonly float Interval = 49;

        //data
        private BBPlayableGraph playableGraph => controllerEditor.PlayableGraph;
        private BehaviorLayer behaviorLayer;

        public void Init(BehaviorControllerEditor Editor, BehaviorLayer _behaviorLayer)
        {
            controllerEditor = Editor;
            behaviorLayer = _behaviorLayer;

            //Init edit layerName
            layerLabel.text = _behaviorLayer.layerName;
            layerText.SetValueWithoutNotify(_behaviorLayer.layerName);
            layerText.RegisterCallback<BlurEvent>(_ =>
            {
                Editor.ApplyModify(() => { behaviorLayer.layerName = layerText.value; }, "Rename layer");
                Editor.RefreshLayerView();
            });

            //drag
            transform.position = new Vector3(0, GetOrder() * Interval, 0);
            DragManipulator = new DragManipulator((e) =>
            {
                //OnDrag
                Dragging = true;
                OriginalIndex = GetOrder(); // 交换前的index
                e.StopImmediatePropagation();
            }, () =>
            {
                //OnDrop
                Dragging = false;
                Tweening = false;
                EditorApplication.update -= TweenLayerViews;

                int currentIndex = GetOrder();
                //复位
                playableGraph.Layers.Remove(behaviorLayer);
                playableGraph.Layers.Insert(OriginalIndex, behaviorLayer);

                if (OriginalIndex != currentIndex)
                {
                    //Undo
                    Editor.ApplyModify(() =>
                    {
                        playableGraph.Layers.Remove(behaviorLayer);
                        playableGraph.Layers.Insert(currentIndex, behaviorLayer);
                    }, "Resort layers");
                }

                float targetY = Interval * currentIndex;
                transform.position = new Vector3(0, targetY, 0);

                //Update currentLayer Index
                controllerEditor.layerIndex = currentIndex;
                controllerEditor.RefreshLayerView();
            }, (e) =>
            {
                //OnMove
                float targetY = transform.position.y + e.y;
                targetY = Mathf.Clamp(targetY, 0, (playableGraph.Layers.Count - 1) * Interval);
                transform.position = new Vector3(0, targetY, 0);

                int index = GetOrder();
                int targetIndex = Mathf.RoundToInt(targetY / Interval);
                if (index != targetIndex)
                {
                    playableGraph.Layers.Remove(behaviorLayer);
                    playableGraph.Layers.Insert(targetIndex, behaviorLayer);
                }

                if (!Tweening)
                {
                    EditorApplication.update += TweenLayerViews;
                }
            });
            this.AddManipulator(DragManipulator);
        }

        #region Drag

        private bool Dragging;
        private int OriginalIndex;
        private DragManipulator DragManipulator;

        private static bool Tweening;

        private void TweenLayerViews()
        {
            Tweening = false;
            EditorApplication.update -= TweenLayerViews;

            var layerViews = LayerViewsContainer.Query<BehaviorLayerView>().ToList();
            foreach (var layerView in layerViews)
            {
                if (!layerView.Dragging)
                {
                    float targetY = layerView.GetOrder() * Interval;
                    float currentY = layerView.transform.position.y;
                    if (Mathf.Abs(currentY - targetY) > 1f)
                    {
                        Tweening = true;
                        targetY = Mathf.Lerp(currentY, targetY, 0.05f);
                    }

                    layerView.transform.position = new Vector3(0, targetY, 0);
                }
            }

            if (Tweening)
            {
                EditorApplication.update += TweenLayerViews;
            }
        }

        #endregion

        private int GetOrder()
        {
            return playableGraph.Layers.IndexOf(behaviorLayer);
        }

        public bool InMiddle(Vector2 worldPosition)
        {
            return worldBound.Contains(worldPosition);
        }

        public void Select()
        {
            AddToClassList("selected");
            BBTimelineSettings.GetSettings().SetActiveObject(behaviorLayer);
            controllerEditor.controllerView.PopulateView(); //刷新graphView
        }

        public void UnSelect()
        {
            RemoveFromClassList("selected");
        }

        public void EditMode(bool inEdit)
        {
            layerLabel.style.display = inEdit? DisplayStyle.None : DisplayStyle.Flex;
            layerText.style.display = inEdit? DisplayStyle.Flex : DisplayStyle.None;
            if (inEdit)
            {
                layerText.Focus();
            }
        }
    }
}