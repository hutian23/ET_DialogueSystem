using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class BehaviorControllerView: GraphView
    {
        public new class UxmlFactory: UxmlFactory<BehaviorControllerView, UxmlTraits>
        {
        }

        public BehaviorControllerEditor Editor;
        private IEnumerable<BehaviorClipView> clipViews => graphElements.OfType<BehaviorClipView>();
        private new IEnumerable<Edge> edges => graphElements.OfType<Edge>();
        private List<BehaviorLayerView> layerViews = new();

        private Vector2 ScreenMousePosition;

        private Vector2 LocalMousePosition
        {
            get
            {
                var mousePosition = Editor.rootVisualElement.ChangeCoordinatesTo(Editor.rootVisualElement.parent,
                    ScreenMousePosition - Editor.position.position);
                return contentViewContainer.WorldToLocal(mousePosition);
            }
        }

        public BehaviorControllerView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContextualMenuManipulator(this.OnContextMenuPopulate));

            var styleSheet =
                    AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Loader/Timeline/Editor/Resources/Style/BehaviorControllerEditor.uss");
            styleSheets.Add(styleSheet);
        }

        #region Edge

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var list = ports.ToList().Where(endPort =>
                    endPort.direction != startPort.direction &&
                    !ExistEdge(startPort, endPort) &&
                    endPort.node != startPort.node).ToList();
            return list;
        }

        //不存在相同连线
        private bool ExistEdge(Port startPort, Port endPort)
        {
            foreach (var edge in edges)
            {
                if ((edge.input == startPort && edge.output == endPort) || (edge.output == startPort && edge.input == endPort))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Clip

        private BehaviorClipView GetClipByGuid(string guid)
        {
            return GetNodeByGuid(guid) as BehaviorClipView;
        }

        //create clipView at mouse point
        private void CreateClip()
        {
            BehaviorClipView clipView = new();
            BehaviorClip clip = new() { Title = "New BehaviorClip", viewDataKey = clipView.viewDataKey, ClipPos = LocalMousePosition };
            clipView.Init(clip);
            AddElement(clipView);

            Editor.ApplyModify(() => { Editor.PlayableGraph.BehaviorClips.Add(clip); }, "Create BehaviorClip");
        }

        private void RemoveClip(BehaviorClipView clipView)
        {
            RemoveElement(clipView);
            Editor.ApplyModify(() => { Editor.PlayableGraph.BehaviorClips.Remove(clipView.BehaviorClip); }, "Delete BehaviorClip", true);
        }

        #endregion

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("create new behaviorClip", _ => { CreateClip(); });
            evt.menu.AppendAction("Reload Behavior", _ => { });
            evt.menu.AppendAction("Preview Behavior", _ => { });
        }

        private void OnContextMenuPopulate(ContextualMenuPopulateEvent evt)
        {
            if (selection.Count == 0) return;
            switch (selection[0])
            {
                case BehaviorClipView clipView:
                {
                    //Root Node
                    if (clipView.viewDataKey == "0")
                    {
                        evt.menu.ClearItems();
                        evt.menu.AppendAction("Open Main Script", _ => { });
                    }
                    else
                    {
                        evt.menu.ClearItems();
                        evt.menu.AppendAction("Open Timeline", _ => { Editor.timelinePlayer.OpenWindow(); });
                        evt.menu.AppendAction("Delete BehaviorClip", _ => { RemoveClip(clipView); });
                    }

                    break;
                }
            }
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            //save nodelink data
            if (graphViewChange.edgesToCreate != null)
            {
                Editor.ApplyModify(() =>
                {
                    foreach (var edge in graphViewChange.edgesToCreate)
                    {
                        BehaviorLinkData linkData = new();
                        linkData.viewDataKey = edge.viewDataKey;
                        linkData.inputGuid = edge.input.node.viewDataKey;
                        linkData.outputGuid = edge.output.node.viewDataKey;

                        Editor.PlayableGraph.linkDatas.Add(linkData);
                    }
                }, "Save node LinkData");
            }

            //save move pos
            if (graphViewChange.movedElements != null)
            {
                var moveClips = new List<BehaviorClipView>();
                foreach (GraphElement element in graphViewChange.movedElements)
                {
                    switch (element)
                    {
                        case BehaviorClipView clipView:
                        {
                            if (clipView.BehaviorClip == null) break;
                            moveClips.Add(clipView);
                            break;
                        }
                    }
                }

                Editor.ApplyModify(() =>
                {
                    foreach (var clipView in moveClips)
                    {
                        clipView.BehaviorClip.ClipPos = clipView.GetPosition().position;
                    }
                }, "Reset Position");
            }

            //Delete
            if (graphViewChange.elementsToRemove != null)
            {
                var removeClips = new List<BehaviorClipView>();
                var removeEdges = new List<Edge>();
                foreach (var eleToRemove in graphViewChange.elementsToRemove)
                {
                    switch (eleToRemove)
                    {
                        case BehaviorClipView clipView:
                            removeClips.Add(clipView);
                            break;
                        case Edge edge:
                            removeEdges.Add(edge);
                            break;
                    }
                }

                Editor.ApplyModify(() =>
                {
                    for (int i = 0; i < removeClips.Count; i++)
                    {
                        BehaviorClipView clipView = removeClips[i];
                        Editor.PlayableGraph.BehaviorClips.Remove(clipView.BehaviorClip);
                        RemoveElement(clipView);
                    }

                    for (int i = 0; i < removeEdges.Count; i++)
                    {
                        Edge removeEdge = removeEdges[i];
                        BehaviorLinkData linkData = Editor.PlayableGraph.linkDatas.FirstOrDefault(link => link.viewDataKey == removeEdge.viewDataKey);
                        Editor.PlayableGraph.linkDatas.Remove(linkData);
                    }
                }, "Remove Element", true);
            }

            return graphViewChange;
        }

        private void Dispose()
        {
            //Dispose nodeview
            foreach (var clipView in clipViews)
            {
                RemoveElement(clipView);
            }

            //Dispose edges
            foreach (var edge in edges)
            {
                RemoveElement(edge);
            }

            //
            Editor.inspectorContainer.Clear();
        }

        public void PopulateView()
        {
            Dispose();

            graphViewChanged -= OnGraphViewChanged;
            graphViewChanged += OnGraphViewChanged;

            //root node
            BehaviorClipView root = new();
            root.capabilities &= ~ Capabilities.Movable;
            root.capabilities &= ~ Capabilities.Deletable;
            root.title = "Root";
            root.viewDataKey = "0";
            root.titleContainer.style.backgroundColor = new Color(0.207f, 0.528f, 0.258f, 0.627f);
            root.inputContainer.Clear();
            root.RegisterCallback<PointerDownEvent>(_ => { BBTimelineSettings.GetSettings().SetActiveObject(Editor.PlayableGraph.root); });
            AddElement(root);

            //create clip view
            foreach (BehaviorClip behaviorClip in Editor.PlayableGraph.BehaviorClips)
            {
                BehaviorClipView behaviorClipView = new();
                behaviorClipView.Init(behaviorClip);
                AddElement(behaviorClipView);
            }

            //create edge
            foreach (var linkData in Editor.PlayableGraph.linkDatas)
            {
                Port input = GetClipByGuid(linkData.inputGuid).Input;
                Port output = GetClipByGuid(linkData.outputGuid).Output;
                Edge edge = output.ConnectTo(input);
                edge.viewDataKey = linkData.viewDataKey;
                AddElement(edge);
            }

            //create layerview
            foreach (var layer in Editor.PlayableGraph.Layers)
            {
                BehaviorLayerView layerView = new();
                this.layerViews.Add(layerView);
                Editor.inspectorContainer.Add(layerView);
            }

            //Regist event
            RegisterCallback<MouseMoveEvent>(evt => { this.ScreenMousePosition = evt.mousePosition + Editor.position.position; });
            RegisterCallback<FocusEvent>(_ =>
            {
                foreach (BehaviorClipView behaviorClipView in clipViews)
                {
                    behaviorClipView.Refresh();
                }
            });
        }
    }
}