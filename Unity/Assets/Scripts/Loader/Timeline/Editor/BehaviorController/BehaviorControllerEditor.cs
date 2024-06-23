using System;
using System.Collections.Generic;
using ET.Client;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    [Serializable]
    public class ParameterInspector
    {
        public void Test()
        {
            Debug.LogWarning("Hello world");
        }

        [Searchable]
        public SharedVariable se;

        [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name", ShowFoldout = true, HideAddButton = true,
            CustomAddFunction = "Test")]
        [HideReferenceObjectPicker, LabelText("Params: ")]
        public List<SharedVariable> Variables = new()
        {
            new SharedVariable() { name = "Hello world", value = 30 },
            new SharedVariable() { name = "Hello_11111" },
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable() { name = "Hello world", value = 30 },
            new SharedVariable() { name = "Hello_11111" },
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable(),
            new SharedVariable()
        };
    }

    [Searchable]
    public class BehaviorControllerEditor: EditorWindow
    {
        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/BehaviorControllerEditor");
            visualTree.CloneTree(root);

            controllerView = root.Q<BehaviorControllerView>();

            #region Layer

            LayersButton = root.Q<Button>("Layers");
            LayersButton.clicked += () => { UpdateInspector(true); };

            layerContainer = root.Q<VisualElement>("layer-container");
            layerContainer.style.display = DisplayStyle.None;

            layerMenuHandler = new DropdownMenuHandler(LayerMenuBuilder);

            layerViewsContainer = root.Q<ScrollView>("layer-views-container");
            layerViewsContainer.RegisterCallback<PointerDownEvent>(PointerDown, TrickleDown.TrickleDown);

            AddLayerButton = root.Q<Button>("add-layer-button");
            AddLayerButton.clicked += AddLayer;

            #endregion

            #region Parameters

            ParametersButton = root.Q<Button>("Parameters");
            ParametersButton.clicked += () => { UpdateInspector(false); };

            parameterContainer = root.Q<ScrollView>("parameter-container");
            parameterContainer.style.display = DisplayStyle.None;

            paramsSearchField = root.Q<ToolbarPopupSearchField>("parameter-search-view");
            paramsSearchField.menu.AppendAction("Hello wolrd", _ => { });

            #endregion

            UpdateInspector(InLayerMode: true);

            controllerView.Editor = this;
        }

        public void OnEnable()
        {
            Undo.undoRedoEvent += OnUndoRedoEvent;
        }

        public void OnDisable()
        {
            Undo.undoRedoEvent -= OnUndoRedoEvent;
        }

        public TimelinePlayer timelinePlayer;
        public BBPlayableGraph PlayableGraph => timelinePlayer.BBPlayable;
        private BehaviorControllerView controllerView;

        #region Inspector

        private void UpdateInspector(bool InLayerMode)
        {
            switch (InLayerMode)
            {
                //Layer mode
                case true:
                {
                    layerContainer.style.display = DisplayStyle.Flex;
                    parameterContainer.style.display = DisplayStyle.None;
                    LayersButton.style.backgroundColor = new StyleColor(new Color(188 / 255f, 188 / 255f, 188 / 255f, 0.2f));
                    ParametersButton.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 0));
                    break;
                }
                //para mode
                case false:
                {
                    layerContainer.style.display = DisplayStyle.None;
                    parameterContainer.style.display = DisplayStyle.Flex;
                    LayersButton.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 0));
                    ParametersButton.style.backgroundColor = new StyleColor(new Color(188 / 255f, 188 / 255f, 188 / 255f, 0.2f));
                    break;
                }
            }
        }

        #endregion

        #region Layer

        private Button LayersButton;
        private VisualElement layerContainer;
        private Button AddLayerButton;
        public ScrollView layerViewsContainer;

        #endregion

        #region Parameters

        private Button ParametersButton;
        private ScrollView parameterContainer;
        private ToolbarPopupSearchField paramsSearchField;

        #endregion

        public static void OpenWindow(TimelinePlayer timelinePlayer)
        {
            BehaviorControllerEditor controllerEditor = GetWindow<BehaviorControllerEditor>();
            controllerEditor.timelinePlayer = timelinePlayer;
            controllerEditor.controllerView.PopulateView();
        }

        #region Undo

        /// <summary>
        /// 需要undoredo的操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="_name"></param>
        /// <param name="refresh">是否执行完操作刷新graphview</param>
        public void ApplyModify(Action action, string _name, bool refresh = false)
        {
            Undo.RegisterCompleteObjectUndo(PlayableGraph, $"Behavior: {_name}");
            PlayableGraph.SerializedUpdate();
            action?.Invoke();
            EditorUtility.SetDirty(PlayableGraph);

            if (refresh)
            {
                controllerView.PopulateView();
            }
        }

        private void OnUndoRedoEvent(in UndoRedoInfo info)
        {
            if (info.undoName.Split(':')[0] == "Behavior")
            {
                controllerView.PopulateView();
            }
        }

        #endregion

        #region Event

        private DropdownMenuHandler layerMenuHandler;
        private Vector2 m_layer_localMousePosition;

        private void LayerMenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Remove Layer", _ =>
            {
                if (PlayableGraph.Layers.Count <= 1)
                {
                    Debug.LogError("PlayableGraph must be at least 1 layer!!!");
                    return;
                }

                var layers = layerViewsContainer.Query<BehaviorLayerView>().ToList();
                foreach (BehaviorLayerView layerView in layers)
                {
                    if (layerView.InMiddle(m_layer_localMousePosition))
                    {
                        ApplyModify(() => { PlayableGraph.Layers.Remove(layerView.behaviorLayer); }, "Remove Layer", true);
                        return;
                    }
                }
            });
        }

        private void PointerDown(PointerDownEvent evt)
        {
            //Select
            if (evt.button == 0)
            {
                var layers = layerViewsContainer.Query<BehaviorLayerView>().ToList();

                bool Selected = false;

                foreach (var layer in layers)
                {
                    layer.UnSelect();
                    if (layer.InMiddle(evt.position) && !Selected)
                    {
                        layer.Select();
                        Selected = true;
                    }
                }
            }
            //Menu
            else if (evt.button == 1)
            {
                m_layer_localMousePosition = evt.position;
                layerMenuHandler.ShowMenu(evt);
            }
        }

        private void AddLayer()
        {
            ApplyModify(() => { PlayableGraph.Layers.Add(new BehaviorLayer() { layerName = "New Layer" }); }, "Add layer", true);
        }

        #endregion
    }
}