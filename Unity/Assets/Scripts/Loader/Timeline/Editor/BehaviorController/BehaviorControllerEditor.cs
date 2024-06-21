using System;
using Timeline;
using Timeline.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorControllerEditor: EditorWindow
{
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/BehaviorControllerEditor");
        visualTree.CloneTree(root);

        controllerView = root.Q<BehaviorControllerView>();

        #region Layer

        layerViewsContainer = root.Q<ScrollView>("layer-views-container");
        layerViewsContainer.RegisterCallback<PointerDownEvent>(evt =>
        {
            Debug.LogWarning(evt.localPosition);
        }, TrickleDown.TrickleDown);
        AddLayerButton = root.Q<Button>("add-layer-button");
        AddLayerButton.clicked += () =>
        {
            ApplyModify(() => { PlayableGraph.Layers.Add(new BehaviorLayer() { layerName = "New Layer" }); }, "Add layer", true);
        };

        #endregion

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
    public ScrollView layerViewsContainer;
    private Button AddLayerButton;

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
}