using System;
using System.Collections.Generic;
using System.Linq;
using ET.Client;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
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

            currentLayerLabel = root.Q<Label>("current-layer-label");

            AddLayerButton = root.Q<Button>("add-layer-button");
            AddLayerButton.clicked += AddLayer;

            #endregion

            #region Parameters

            ParametersButton = root.Q<Button>("Parameters");
            ParametersButton.clicked += () => { UpdateInspector(false); };

            parameterContainer = root.Q<VisualElement>("parameter-container");
            parameterContainer.style.display = DisplayStyle.None;

            //param view
            parameterViewContainer = root.Q<VisualElement>("parameter-view-container");
            parameterViewContainer.RegisterCallback<PointerDownEvent>(ParamViewOnPointerDown, TrickleDown.TrickleDown);
            paramViewMenuHandler = new DropdownMenuHandler(ParamMenuBuilder);

            //Add parameters
            addParameterButton = root.Q<Button>("parameters-add-button");
            dropdownMenuManipulator = new DropdownMenuHandler(AddParams);
            addParameterButton.clickable.clicked += () => { dropdownMenuManipulator.ShowMenu(addParameterButton); };

            //search
            paramsSearchField = root.Q<ToolbarPopupSearchField>("parameter-search-view");
            paramsSearchField.RegisterValueChangedCallback(_ => { RefreshParamView(); });
            var paramsSearchBtn = paramsSearchField.Q<Button>("unity-search");
            paramTypeMenuHandler = new DropdownMenuHandler(ShowParamSearchMenu);
            paramsSearchBtn.clickable.clicked += () => { paramTypeMenuHandler.ShowMenu(paramsSearchBtn); };

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
        public BehaviorControllerView controllerView;

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
        private Label currentLayerLabel;
        public ScrollView layerViewsContainer;

        private int _layerIndex;

        public int layerIndex
        {
            get
            {
                //adjust layerView
                if (_layerIndex >= PlayableGraph.Layers.Count)
                {
                    _layerIndex = 0;
                }

                return _layerIndex;
            }
            set
            {
                _layerIndex = value;
            }
        }

        public BehaviorLayer currentLayer => PlayableGraph.Layers[layerIndex];

        public void RefreshLayerView()
        {
            layerViewsContainer.Clear();
            layerViewsContainer.ForceScrollViewUpdate();

            //refresh current layer
            currentLayerLabel.text = currentLayer.layerName;

            //inspector create layerview
            for (int i = 0; i < PlayableGraph.Layers.Count; i++)
            {
                BehaviorLayer layer = PlayableGraph.Layers[i];

                BehaviorLayerView layerView = new();
                layerView.Init(this, layer);
                layerViewsContainer.Add(layerView);

                if (i == layerIndex)
                {
                    layerView.Select();
                }
            }
        }

        private DropdownMenuHandler layerMenuHandler;

        private void LayerMenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Edit Layer", _ =>
            {
                var layers = layerViewsContainer.Query<BehaviorLayerView>().ToList();
                layers[layerIndex].EditMode(true);
            });
            menu.AppendAction("Remove Layer", _ =>
            {
                if (PlayableGraph.Layers.Count <= 1)
                {
                    Debug.LogError("PlayableGraph must be at least 1 layer!!!");
                    return;
                }

                ApplyModify(() => { PlayableGraph.Layers.RemoveAt(layerIndex); }, "Remove Layer");
                RefreshLayerView();
            });
        }

        private void PointerDown(PointerDownEvent evt)
        {
            //Select
            if (evt.button == 0)
            {
                var layers = layerViewsContainer.Query<BehaviorLayerView>().ToList();

                for (int i = 0; i < layers.Count; i++)
                {
                    BehaviorLayerView layer = layers[i];
                    layer.UnSelect();
                    if (layer.InMiddle(evt.position))
                    {
                        layer.Select();
                        layerIndex = i;
                    }
                }
            }
            //Menu
            else if (evt.button == 1)
            {
                layerMenuHandler.ShowMenu(evt);
            }
        }

        private void AddLayer()
        {
            ApplyModify(() => { PlayableGraph.Layers.Add(new BehaviorLayer() { layerName = "New Layer" }); }, "Add layer");
            RefreshLayerView();
        }

        #endregion

        #region Parameters

        private Button ParametersButton;
        private VisualElement parameterContainer;
        public VisualElement parameterViewContainer;
        private Button addParameterButton;
        private ToolbarPopupSearchField paramsSearchField;

        private DropdownMenuHandler dropdownMenuManipulator; //search window
        private DropdownMenuHandler paramTypeMenuHandler; // add view
        private DropdownMenuHandler paramViewMenuHandler; // click on paramView

        public string SearchParamMode = "Name";

        private BehaviorParamView SelectParamView;

        private void AddParams(DropdownMenu menu)
        {
            foreach (var param in BBTimelineEditorUtility.ParamsTypeDict)
            {
                menu.AppendAction(param.Key, _ =>
                {
                    ApplyModify(() => { PlayableGraph.Parameters.Add(new SharedVariable() { name = "New Param", value = param.Value }); },
                        "Add Param");
                    RefreshParamView();
                });
            }
        }

        private void ParamViewOnPointerDown(PointerDownEvent evt)
        {
            //Select
            if (evt.button == 0)
            {
                var parameters = parameterViewContainer.Query<BehaviorParamView>().ToList();

                foreach (var param in parameters)
                {
                    param.UnSelect();
                    if (param.InMiddle(evt.position))
                    {
                        SelectParamView = param;
                    }
                }

                SelectParamView?.Select();
            }
            else if (evt.button == 1)
            {
                paramViewMenuHandler.ShowMenu(evt);
            }
        }

        private void ParamMenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Edit", _ => { SelectParamView.InEditMode(true); });
            menu.AppendAction("Remove",
                _ =>
                {
                    ApplyModify(() => { PlayableGraph.Parameters.Remove(SelectParamView.variable); }, "Remove param");
                    RefreshParamView();
                });
        }

        private void ShowParamSearchMenu(DropdownMenu menu)
        {
            menu.AppendAction("Name", _ =>
                {
                    SearchParamMode = "Name";
                    RefreshParamView();
                },
                SearchParamMode.Equals("Name")? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);
            foreach (var param in BBTimelineEditorUtility.ParamsTypeDict)
            {
                menu.AppendAction(param.Key, _ =>
                    {
                        SearchParamMode = param.Key;
                        RefreshParamView();
                    },
                    SearchParamMode.Equals(param.Key)? DropdownMenuAction.Status.Checked : DropdownMenuAction.Status.Normal);
            }
        }

        private List<SharedVariable> GetParams()
        {
            if (SearchParamMode == "Name")
            {
                return FuzzySearch(PlayableGraph.Parameters);
            }

            BBTimelineEditorUtility.ParamsTypeDict.TryGetValue(SearchParamMode, out object o);
            if (o == null)
            {
                Debug.LogError($"not found search type:{SearchParamMode}");
                return null;
            }

            Type objType = o.GetType();
            return FuzzySearch(PlayableGraph.Parameters.Where(v => v.value.GetType() == objType).ToList());
        }

        private List<SharedVariable> FuzzySearch(List<SharedVariable> variables)
        {
            var _searchTerm = paramsSearchField.value.ToLower();
            //空搜索
            if (string.IsNullOrEmpty(_searchTerm))
            {
                return variables;
            }

            var results = variables.Where(item => item.name.ToLower().Contains(_searchTerm)) //进行包含匹配并忽略大小写
                    .OrderBy(item => item.name.Length)
                    .ToList();
            return results;
        }

        public void RefreshParamView()
        {
            parameterViewContainer.Clear();
            //Create paramView
            foreach (var param in GetParams())
            {
                Activator.CreateInstance(BBTimelineEditorUtility.ParamsFieldDict[param.value.GetType()], param, this);
            }
        }

        #endregion

        public static void OpenWindow(TimelinePlayer timelinePlayer)
        {
            BehaviorControllerEditor controllerEditor = GetWindow<BehaviorControllerEditor>();
            controllerEditor.timelinePlayer = timelinePlayer;
            //添加默认layer
            if (timelinePlayer.BBPlayable.Layers.Count == 0)
            {
                timelinePlayer.BBPlayable.Layers.Add(new BehaviorLayer());
            }
            controllerEditor.RefreshView();
        }

        public void RefreshView()
        {
            RefreshLayerView();
            RefreshParamView();
            controllerView.PopulateView();
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
                RefreshView();
            }
        }

        private void OnUndoRedoEvent(in UndoRedoInfo info)
        {
            if (info.undoName.Split(':')[0] == "Behavior")
            {
                RefreshView();
            }
        }

        #endregion
    }
}