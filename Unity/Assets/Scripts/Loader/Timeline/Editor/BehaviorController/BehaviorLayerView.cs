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

            layerLabel = this.Q<TextField>("layer-label");
        }

        private DropdownMenuManipulator MenuManipulator;
        private BehaviorControllerEditor controllerEditor;
        private BBPlayableGraph playableGraph => controllerEditor.PlayableGraph;
        private ScrollView LayerViewsContainer => controllerEditor.layerViewsContainer;
        private readonly TextField layerLabel;

        private BehaviorLayer behaviorLayer;
        private readonly float Interval = 49;

        public void Init(BehaviorControllerEditor Editor, BehaviorLayer _behaviorLayer)
        {
            controllerEditor = Editor;
            behaviorLayer = _behaviorLayer;

            //Init edit layerName
            layerLabel.SetValueWithoutNotify(_behaviorLayer.layerName);
            layerLabel.RegisterCallback<BlurEvent>(_ =>
            {
                Editor.ApplyModify(() => { behaviorLayer.layerName = layerLabel.value; }, "Rename Clip");
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
    }
}